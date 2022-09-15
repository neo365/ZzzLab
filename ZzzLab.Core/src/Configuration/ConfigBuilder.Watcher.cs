using System;
using System.Collections.Generic;
using System.IO;
using ZzzLab.IO;

namespace ZzzLab.Configuration
{
    public partial class ConfigBuilder
    {
        private Dictionary<string, FileSystemWatcher> DicWatcher;

        public IConfigBuilder AddWatcher(string filePath)
        {
            if (File.Exists(filePath) == false) throw new FileNotFoundException(null, filePath);

            if (DicWatcher == null) DicWatcher = new Dictionary<string, FileSystemWatcher>();

            filePath = Path.GetFullPath(filePath); // 절대경로로 변경

            if (DicWatcher.ContainsKey(filePath)) return this;

            string fileDir = filePath;
            string fileName = "*.*";

            NotifyFilters filters = NotifyFilters.Attributes
                        | NotifyFilters.CreationTime
                        | NotifyFilters.FileName
                        | NotifyFilters.LastAccess
                        | NotifyFilters.LastWrite
                        | NotifyFilters.Security
                        | NotifyFilters.Size;

            if (filePath.IsDirectory())
            {
                filters = filters
                    | NotifyFilters.LastWrite
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName;
            }
            else
            {
                fileDir = Path.GetDirectoryName(filePath);
                fileName = Path.GetFileName(filePath);
            }

            FileSystemWatcher watcher = new FileSystemWatcher(fileDir, fileName)
            {
                NotifyFilter = filters,
                IncludeSubdirectories = filePath.IsDirectory()
            };

            watcher.Changed += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.EnableRaisingEvents = true;

            DicWatcher.Add(filePath, watcher);

            return this;
        }

        public IConfigBuilder RemoveWatcher(string filePath)
        {
            FileSystemWatcher Watcher = DicWatcher[filePath];

            if (Watcher != null) Watcher.EnableRaisingEvents = false;

            DicWatcher.Remove(filePath);

            return this;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;

            Logger.Info($"Changed: {e.FullPath}");

            Configurator.Instance?.Reloading();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
            => Logger.Info($"Created: {e.FullPath}");

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Logger.Info($"Deleted: {e.FullPath}");
            RemoveWatcher(e.FullPath);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Logger.Info($"Renamed: {e.OldFullPath} => {e.FullPath}");

            RemoveWatcher(e.OldFullPath);

            Configurator.Instance?.Reloading();
        }

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