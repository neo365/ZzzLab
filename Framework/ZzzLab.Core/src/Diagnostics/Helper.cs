using System;
using System.Diagnostics;

namespace ZzzLab.Diagnostics
{
    public static class Helper
    {
        public static bool OpenURL(this string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = url;
                    process.Start();

                    //process.WaitForExit();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return false;
        }
    }
}