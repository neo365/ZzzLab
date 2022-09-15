using System;
using System.IO;

namespace ZzzLab.Configuration
{
    /// <summary>
    /// 환경설정파일의 변경점을 확인하기 위한 Class
    /// </summary>
    public class ConfigWatcher
    {
        private readonly FileSystemWatcher Watcher;

        /// <summary>
        /// 환경설정파일의 변경점을 확인하기 위한 Class
        /// </summary>
        /// <param name="filePath">감시할 파일 경로</param>
        /// <exception cref="FileNotFoundException">파일이 지정된 위치에 없을때</exception>
        public ConfigWatcher(string filePath)
        {
            if (File.Exists(filePath) == false) throw new FileNotFoundException(filePath);

            Watcher = new FileSystemWatcher(filePath)
            {
                NotifyFilter = NotifyFilters.Attributes
                    | NotifyFilters.CreationTime
                    | NotifyFilters.FileName
                    | NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.Security
                    | NotifyFilters.Size
            };

            Watcher.Changed += OnChanged;
            Watcher.Created += OnCreated;
            Watcher.Deleted += OnDeleted;
            Watcher.Renamed += OnRenamed;
            Watcher.Error += OnError;

            Watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;

            Logger.Info($"Changed: {e.FullPath}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
            => Logger.Info($"Created: {e.FullPath}");

        private void OnDeleted(object sender, FileSystemEventArgs e)
            => Logger.Info($"Deleted: {e.FullPath}");

        private void OnRenamed(object sender, RenamedEventArgs e)
            => Logger.Info($"Renamed: {e.OldFullPath} => {e.FullPath}");

        private void OnError(object sender, ErrorEventArgs e)
            => PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex == null) return;

            Logger.Error(ex);
            if (ex.InnerException != null)
            {
                Logger.Error(ex.InnerException);
            }
        }
    }
}