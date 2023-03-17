using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ZzzLab.IO
{
    public partial class RestartManager
    {
        #region p/invoke

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern int RmRegisterResources(ulong pSessionHandle,
            uint nFiles,
            string[] rgsFilenames,
            uint nApplications,
            [In] RM_UNIQUE_PROCESS[] rgApplications,
            uint nServices,
            string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern int RmStartSession(out ulong pSessionHandle, ulong dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmEndSession(ulong pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmGetList(ulong dwSessionHandle,
            out uint pnProcInfoNeeded,
            ref int pnProcInfo,
            [In][Out] RM_PROCESS_INFO[] rgAffectedApps,
            ref uint lpdwRebootReasons);

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct RM_UNIQUE_PROCESS
        {
            public readonly int dwProcessId;
            public readonly FILETIME ProcessStartTime;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private readonly struct RM_PROCESS_INFO
        {
            public readonly RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public readonly string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public readonly string strServiceShortName;

            public readonly RM_APP_TYPE ApplicationType;
            public readonly RM_APP_STATUS AppStatus;
            public readonly int TSSessionId;

            [MarshalAs(UnmanagedType.Bool)] public readonly bool bRestartable;
        }

        [Flags]
        private enum RM_APP_STATUS
        {
            RmStatusUnknown = 0,
            RmStatusRunning = 1,
            RmStatusStopped = 2,
            RmStatusStoppedOther = 4,
            RmStatusRestarted = 8,
            RmStatusErrorOnStop = 16, // 0x00000010
            RmStatusErrorOnRestart = 32, // 0x00000020
            RmStatusShutdownMasked = 64, // 0x00000040
            RmStatusRestartMasked = 128 // 0x00000080
        }

        private enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000 // 0x000003E8
        }

        #endregion p/invoke

        public RestartManager()
        {
        }

        public static RestartManager Create()
            => new RestartManager();

        public IReadOnlyList<LockingProcessInfo> GetLockingProcessesInfo(string filePath, bool throwOnError = true)
        {
            IEnumerable<Process> result = GetLockingProcesses(filePath, false, throwOnError, out _);
            List<LockingProcessInfo> list = new List<LockingProcessInfo>();
            foreach (Process process in result)
            {
                list.Add(LockingProcessInfo.Create(process));
            }

            return list;
        }

        public IReadOnlyList<Process> GetLockingProcesses(string filePath, bool throwOnError = true)
           => GetLockingProcesses(filePath, false, throwOnError, out _);

        public IReadOnlyList<Process> GetLockingProcesses(
            string filePath,
            bool onlyRestartable,
            bool throwOnError,
            out Exception error)
            => GetLockingProcesses(filePath?.ToIEnumerable(), onlyRestartable, throwOnError, out error);

        public IReadOnlyList<Process> GetLockingProcesses(
            IEnumerable<string> filePaths,
            bool onlyRestartable,
            bool throwOnError,
            out Exception error)
        {
            if (filePaths == null) throw new ArgumentNullException(nameof(filePaths));

            foreach (string filePath in filePaths)
            {
                if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
                if (File.Exists(filePath) == false) throw new FileNotFoundException("File Not Found", filePath);
            }

            var lockingProcesses = new List<Process>();
            var stringList = new List<string>(filePaths);
            var strSessionKey = Guid.NewGuid().ToString();
            int errorCode = RmStartSession(out var pSessionHandle, 0, strSessionKey);
            if (errorCode != 0)
            {
                error = new Win32Exception(errorCode);
                if (throwOnError) throw error;
                return lockingProcesses;
            }

            try
            {
                errorCode = RmRegisterResources(pSessionHandle, (uint)stringList.Count, stringList.ToArray(), 0, null, 0, null);
                if (errorCode != 0)
                {
                    error = new Win32Exception(errorCode);
                    if (throwOnError) throw error;
                    return lockingProcesses;
                }

                var pnProcInfo = 0;
                uint lpdwRebootReasons = 0;
                errorCode = RmGetList(pSessionHandle, out var pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
                switch (errorCode)
                {
                    case 0:
                        error = null;
                        return lockingProcesses;

                    case 234:
                        var rgAffectedApps = new RM_PROCESS_INFO[pnProcInfoNeeded];
                        pnProcInfo = rgAffectedApps.Length;
                        int errorCode2 = RmGetList(pSessionHandle, out pnProcInfoNeeded, ref pnProcInfo, rgAffectedApps, ref lpdwRebootReasons);
                        if (errorCode2 != 0)
                        {
                            error = new Win32Exception(errorCode2);
                            if (throwOnError) throw error;
                            return lockingProcesses;
                        }

                        for (var index = 0; index < pnProcInfo; ++index)
                            try
                            {
                                if (rgAffectedApps[index].bRestartable == false && onlyRestartable) continue;
                                lockingProcesses.Add(Process.GetProcessById(rgAffectedApps[index].Process.dwProcessId));
                            }
                            catch (Exception ex)
                            {
                                error = ex;
                                return lockingProcesses;
                            }

                        error = null;
                        return lockingProcesses;

                    default:
                        error = new Win32Exception(errorCode);
                        if (throwOnError) throw error;
                        return lockingProcesses;
                }
            }
            finally
            {
                _ = RmEndSession(pSessionHandle);
            }
        }
    }

    public class LockingProcessInfo
    {
        public Process Process { get; private set; }

        public string Name
        {
            get => Process.ProcessName;
        }

        public int Id
        {
            get => Process.Id;
        }

        public string FilePath
        {
            get => Process.MainModule.FileName;
        }

        public LockingProcessInfo(Process process)
        {
            this.Process = process ?? throw new ArgumentNullException(nameof(process));
        }

        public static LockingProcessInfo Create(Process process)
            => new LockingProcessInfo(process);
    }
}