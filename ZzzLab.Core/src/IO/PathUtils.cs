using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ZzzLab.IO
{
    public static class PathUtils
    {
        public static string GetAppDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName));
        }

        public static string GetTempFilePath(string ext = "")
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ext);
        }

        public static string GetAppTempFilePath(string ext = "", string header = "")
        {
            header = string.IsNullOrWhiteSpace(header) ? Path.GetRandomFileName() : header.ToValidFileName();

            string appendstr = (string.IsNullOrEmpty(header?.Trim()) ? "" : "_");
            string appDataDir = GetAppDataPath();
            string fileName = string.Format("{0}{1}{2}{3}", header, appendstr, DateTime.Now.ToString("yyyyMMddHHmmssfff"), ext);

            return Path.Combine(appDataDir, fileName);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void HasFileCheck(string filePath, bool isOverwrite = true)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            else if (File.Exists(filePath))
            {
                if (isOverwrite) FileDelete(filePath);
                else throw new FileFoundException(filePath);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void FileDelete(string filePath)
            => File.Delete(filePath);

        public static string GetTargetPath(string inputPath, string outputPath, string ext)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                string dirPath = Path.GetDirectoryName(inputPath);
                if (string.IsNullOrWhiteSpace(dirPath)) return null;
                return Path.Combine(
                   dirPath,
                    $"{Path.GetFileNameWithoutExtension(inputPath)}{ext}");
            }
            else if (string.IsNullOrEmpty(Path.GetDirectoryName(outputPath)))
            {
                outputPath = Path.Combine(Environment.CurrentDirectory, outputPath);
            }

            return Path.GetFullPath(outputPath);
        }

        public static bool CheckDirectory(this string dir, bool isCreate = true)
        {
            if (string.IsNullOrWhiteSpace(dir)) return false;
            DirectoryInfo dirinfo = new DirectoryInfo(dir);

            return dirinfo.CheckDirectory(isCreate);
        }

        public static bool CheckDirectory(this DirectoryInfo directoryInfo, bool isCreate = true)
        {
            if (directoryInfo == null) return false;

            if (directoryInfo.Exists) return true;
            else
            {
                if (isCreate)
                {
                    CheckDirectory(directoryInfo.Parent, true);
                    Directory.CreateDirectory(directoryInfo.FullName);
                    return true;
                }
            }

            return false;
        }

        public static string GetValidFileName(this string fileName)
            => fileName.Replace(Path.GetInvalidFileNameChars(), '_');

        public static string GetAvailableDirName(this string folderPath)
            => folderPath.Replace(Path.GetInvalidPathChars(), '_');

        /// <summary>
        /// 사용가능한 파일명을 가져온다. 이미 있는 파일의 경우 파일명에 (1), (2)를 붙힌다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetAvailableFilePath(string filePath)
            => GetAvailableFilePath(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

        /// <summary>
        /// 사용가능한 파일명을 가져온다. 이미 있는 파일의 경우 파일명에 (1), (2)를 붙힌다.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetAvailableFilePath(string folderPath, string filename)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));

            folderPath = folderPath.GetAvailableDirName();
            filename = filename.GetValidFileName();

            string fullPath = Path.Combine(folderPath, filename);
            string filenameWithoutExtention = Path.GetFileNameWithoutExtension(filename);
            string extension = Path.GetExtension(filename);

            int nextNumberOfCopy = 1;

            while (File.Exists(fullPath))
            {
                fullPath = Path.Combine(folderPath, $"{filenameWithoutExtention}({nextNumberOfCopy++}){extension}");
            }
            return fullPath;
        }

        public static char[] GetFirstAvailableDrive()
        {
            // these are the driveletters that are in use;
            var usedDriveLetters =
                from drive
                in DriveInfo.GetDrives()
                select drive.Name.ToUpperInvariant()[0];

            // these are all possible driveletters [D..Z] that
            // we can choose from (don't want "B" as drive);
            string allDrives = string.Empty;
            for (char c = 'D'; c < 'Z'; c++)
            {
                allDrives += c.ToString();
            }
            // these are the ones that are available;
            var availableDriveLetters = allDrives.Except(usedDriveLetters);

            if (availableDriveLetters.Any() == false) return null;

            return availableDriveLetters.ToArray();
        }

        public static char GetFirstAvailableDriveLetter()
        {
            char[] availableDriveLetters = GetFirstAvailableDrive();

            if (availableDriveLetters == null || availableDriveLetters.Length == 0) throw new DriveNotFoundException("No drives available!");

            return availableDriveLetters.First();
        }

        public static string GetFirstAvailableMountPoint() => $"{GetFirstAvailableDriveLetter()}:\\";

        public static string RemoveDriveLetter(this string f) => (f.IndexOf(":") > 0 ? f.Substring(f.IndexOf(":") + 1) : f);

        private static long GetDirectorySize(this DirectoryInfo dir)
        {
            if (dir == null) throw new ArgumentNullException(nameof(dir));

            return dir.GetFiles().Sum(fi => fi.Length) +
                   dir.GetDirectories().Sum(di => GetDirectorySize(di));
        }

        public static long GetDirectorySize(string dirPath) => GetDirectorySize(new DirectoryInfo(dirPath));

        public static long GetFileSize(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            if (Directory.Exists(filePath)) GetDirectorySize(filePath);
            else if (File.Exists(filePath) == false) throw new FileNotFoundException(filePath);

            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Attributes.HasFlag(FileAttributes.Directory)) return GetDirectorySize(filePath);
            else return fileInfo.Length;
        }

        public static bool Exists(this string filePath)
        {
            if (File.Exists(filePath) == false) return false;

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Attributes.HasFlag(FileAttributes.Directory)) return Directory.Exists(filePath);
            return fileInfo.Exists;
        }

        public static bool IsDirectory(this string filePath)
        {
            if (File.Exists(filePath) == false) throw new FileNotFoundException(null, filePath);
            return (File.GetAttributes(filePath).HasFlag(FileAttributes.Directory));
        }

        public static bool IsFile(this string filePath)
        {
            if (File.Exists(filePath) == false) throw new FileNotFoundException(null, filePath);
            return (File.GetAttributes(filePath).HasFlag(FileAttributes.Directory));
        }
    }
}