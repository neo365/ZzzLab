using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace ZzzLab.IO
{
    public static partial class FileExtension
    {
        public static bool Save(this string s, string filePath, bool overwrite = true, Encoding encoding = null, int secondsToWait = 0)
        {
            if (string.IsNullOrEmpty(filePath?.Trim())) throw new ArgumentNullException(nameof(filePath));
            encoding = encoding ?? Encoding.Default;

            try
            {
                // 지연저장 :  0 이면 무시
                if (secondsToWait > 0)
                {
                    if (filePath.IsFileLockedWait(secondsToWait)) return false;
                }
                if (overwrite == false && File.Exists(filePath)) return false;

                File.WriteAllText(filePath, s, encoding);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Save(this byte[] bytes, string filePath, int secondsToWait = 0)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            try
            {
                // 지연저장 :  0 이면 무시
                if (secondsToWait > 0)
                {
                    if (filePath.IsFileLockedWait(secondsToWait)) return false;
                }

                File.WriteAllBytes(filePath, bytes);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Append(this string s, string filePath, Encoding encoding = null, int secondsToWait = 0)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            encoding = encoding ?? Encoding.Default;

            try
            {
                // 지연저장 :  0 이면 무시
                if (secondsToWait > 0)
                {
                    if (filePath.IsFileLockedWait(secondsToWait)) return false;
                }

                if (File.Exists(filePath))
                {
                    using (StreamWriter file = new StreamWriter(filePath, true, encoding))
                    {
                        file.WriteLine(s);
                    }
                }
                else File.WriteAllText(filePath, s, encoding);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(this string filePath, int secondsToWait = 0)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            try
            {
                if (string.IsNullOrEmpty(filePath)) return false;
                if (File.Exists(filePath) == false) return false;
                if (secondsToWait > 0)
                {
                    if (filePath.IsFileLockedWait(secondsToWait)) return false;
                }

                PathUtils.FileDelete(filePath);

                return (File.Exists(filePath) == false);
            }
            catch
            {
                return false;
            }
        }

        public static string GetRename(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            string renamePath = filePath;

            string fileName;
            string fileExtension;
            string directoryName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrWhiteSpace(directoryName)) return null;

            if (filePath.IsDirectory())
            {
                fileName = Path.GetFileName(filePath);
                fileExtension = "";
            }
            else
            {
                fileName = Path.GetFileNameWithoutExtension(filePath);
                fileExtension = Path.GetExtension(filePath);
            }

            int duplicate_count = 0;

            while (renamePath.Exists())
            {
                duplicate_count++;
                string newFileName = $"{fileName}({duplicate_count}){fileExtension}";
                renamePath = Path.Combine(directoryName, newFileName);
            }

            return renamePath;
        }

        public static void Touch(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath?.Trim())) throw new ArgumentNullException(nameof(filePath));

            using (FileStream fs = File.Create(filePath)) { }
        }

        #region File Locker

        public static bool IsFileLockedWait(this string filePath, int secondsToWait = 1)
        {
            bool isLocked = true;
            int i = 0;
            while (isLocked && ((i < secondsToWait) || (secondsToWait == 0)))
            {
                try
                {
                    using (File.Open(filePath, FileMode.Open)) { }
                    return false;
                }
                catch (IOException ex)
                {
                    isLocked = IsFileLocked(ex);

                    i++;
                    if (secondsToWait != 0)
                    {
                        using (var wait = new System.Threading.ManualResetEvent(false))
                        {
                            wait.WaitOne(1000);
                        }
                    }
                }
            }

            return isLocked;
        }

        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException ex)
            {
                return IsFileLocked(ex);
            }

            return false;
        }

        public static bool IsFileLocked(this IOException ex)
        {
#if false
            var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
            return errorCode == 32 || errorCode == 33;

#else
            const uint HRFileLocked = 0x80070020;
            const uint HRPortionOfFileLocked = 0x80070021;

            var errorCode = (uint)Marshal.GetHRForException(ex);
            return errorCode == HRFileLocked || errorCode == HRPortionOfFileLocked;
#endif
        }

        public static bool IsFileLockedWhois(IOException ex)
        {
#if false
            var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
            return errorCode == 32 || errorCode == 33;
#else
            const uint HRFileLocked = 0x80070020;
            const uint HRPortionOfFileLocked = 0x80070021;

            var errorCode = (uint)Marshal.GetHRForException(ex);
            return errorCode == HRFileLocked || errorCode == HRPortionOfFileLocked;
#endif
        }

        #endregion File Locker

        public static bool IsValidFileName(this string name)
        {
            return (name.IndexOf("\\") < 0
                && name.IndexOf("/") < 0
                && name.IndexOf(":") < 0
                && name.IndexOf("*") < 0
                && name.IndexOf("?") < 0
                && name.IndexOf("\"") < 0
                && name.IndexOf("<") < 0
                && name.IndexOf(">") < 0
                && name.IndexOf("|") < 0);
        }

        public static string ToValidFileName(this string name)
        {
            return name
                .Replace("\\", "_")
                .Replace("/", "_")
                .Replace(":", "_")
                .Replace("*", "_")
                .Replace("?", "_")
                .Replace("\"", "_")
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace("|", "_");
        }

        public static string GetMD5HashKey(this string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string GetSha2565HashKey(this string filePath)
        {
            using (SHA256 Sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    var hash = Sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static IEnumerable<string> FileAttributesToArray(this FileAttributes attributes)
        {
            List<string> attrList = new List<string>();

            if (attributes.HasFlag(FileAttributes.ReadOnly)) attrList.Add(FileAttributes.ReadOnly.ToString());
            if (attributes.HasFlag(FileAttributes.Hidden)) attrList.Add(FileAttributes.Hidden.ToString());
            if (attributes.HasFlag(FileAttributes.System)) attrList.Add(FileAttributes.System.ToString());
            if (attributes.HasFlag(FileAttributes.Directory)) attrList.Add(FileAttributes.Directory.ToString());
            if (attributes.HasFlag(FileAttributes.Archive)) attrList.Add(FileAttributes.Archive.ToString());
            if (attributes.HasFlag(FileAttributes.Device)) attrList.Add(FileAttributes.Device.ToString());
            if (attributes.HasFlag(FileAttributes.Normal)) attrList.Add(FileAttributes.Normal.ToString());
            if (attributes.HasFlag(FileAttributes.Temporary)) attrList.Add(FileAttributes.Temporary.ToString());
            if (attributes.HasFlag(FileAttributes.SparseFile)) attrList.Add(FileAttributes.SparseFile.ToString());
            if (attributes.HasFlag(FileAttributes.ReparsePoint)) attrList.Add(FileAttributes.ReparsePoint.ToString());
            if (attributes.HasFlag(FileAttributes.Compressed)) attrList.Add(FileAttributes.Compressed.ToString());
            if (attributes.HasFlag(FileAttributes.Offline)) attrList.Add(FileAttributes.Offline.ToString());
            if (attributes.HasFlag(FileAttributes.NotContentIndexed)) attrList.Add(FileAttributes.NotContentIndexed.ToString());
            if (attributes.HasFlag(FileAttributes.Encrypted)) attrList.Add(FileAttributes.Encrypted.ToString());
            if (attributes.HasFlag(FileAttributes.IntegrityStream)) attrList.Add(FileAttributes.IntegrityStream.ToString());
            if (attributes.HasFlag(FileAttributes.NoScrubData)) attrList.Add(FileAttributes.NoScrubData.ToString());

            return attrList.Distinct().ToList();
        }
    }
}