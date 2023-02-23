using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using ZzzLab.Logging;

namespace ZzzLab.MicroServer.Views
{
    public partial class MainWindow
    {
        #region for Debug Logs

        private readonly DebugWindow _Logs = new DebugWindow();

        private void InitializeDebugger()
        {
            AppHelper.AppLogger.Message += LoggerMessage;
        }

        public void AppendTrace(LogLevel level, object value, [CallerMemberName] string? methodName = null)
            => AppendDebug($"[{DateTime.Now.To24Hours()} | {level}]{methodName} : {value}");

        public void AppendDebug() => AppendDebug(null);

        public void AppendDebug(string? text)
        {
            Dispatcher.Invoke(() =>
            {
                _Logs.logText.AppendText(Environment.NewLine + text);
                _Logs.logText.ScrollToEnd();
            }, DispatcherPriority.Background);
        }

        public void ShowDebug()
        {
            Dispatcher.Invoke(() =>
            {
                _Logs.Show();
            });
        }

        internal void LoggerMessage(object sender, LogEventArgs e)
            => AppendDebug($"{e.LogDateTime.To24Hours()} | {e.Level,-7} | {e.MethodName,-20} | {e.Value}");

        #endregion for Debug Logs
    }
}