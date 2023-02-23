using System;
using System.Diagnostics;
using System.Windows.Input;
using ZzzLab.Web;
using Forms = System.Windows.Forms;

namespace ZzzLab.MicroServer.Context
{
    internal partial class MainContext
    {
        public ICommand? AboutCommand { protected set; get; }
        public ICommand? DebugCommand { protected set; get; }
        public ICommand? UrlOpenCommand { protected set; get; }
        public ICommand? HelpCommand { protected set; get; }
        public ICommand? HideCommand { protected set; get; }
        public ICommand? ServerCommand { protected set; get; }
        public ICommand? ExitCommand { protected set; get; }

        protected override void InitializeCommand()
        {
            AboutCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    this.Owner.ShowWindow();
                },
                CanExecuteDelegate = param => true
            };

            DebugCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    this.Owner.ShowDebug();
                },
                CanExecuteDelegate = param => true
            };

            UrlOpenCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    string site = param.ToString() ?? string.Empty;

                    switch (site.ToLower())
                    {
                        case "open":
                            OpenURL($"http://localhost:{AppConstant.WebPort}");
                            break;

                        case "swagger":
                            OpenURL($"http://localhost:{AppConstant.WebPort}/swagger/index.html");
                            break;

                        default:
                            Forms.MessageBoxEx.Error("지정된 URL이 없습니다.");
                            break;
                    }
                },
                CanExecuteDelegate = param => true
            };

            HelpCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    // Do Nothing
                },
                CanExecuteDelegate = param => true
            };

            HideCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    this.Owner.HideWindow();
                },
                CanExecuteDelegate = param => true
            };

            ServerCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    string cmd = param.ToString() ?? string.Empty;

                    switch (cmd.ToLower())
                    {
                        case "start":
                            WebHostHelper.Start();
                            break;

                        case "restart":
                            WebHostHelper.Restart();
                            break;

                        case "stop":
                            WebHostHelper.Stop();
                            break;

                        default:
                            Forms.MessageBoxEx.Error("지정된 URL이 없습니다.");
                            break;
                    }
                },
                CanExecuteDelegate = param => true
            };

            ExitCommand = new CustomCommand()
            {
                ExecuteDelegate = param =>
                {
                    try
                    {
                        if (Forms.MessageBoxEx.YesNo($"정말로 종료하시겠습니까?\n종료후 일부 프로그램이 동작하지 않을수 있습니다.", "경고"))
                        {
                            this.Owner.IsClosing = true;
                            this.Owner.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Forms.MessageBoxEx.Error(ex.Message);
                    }
                    finally { }
                },
                CanExecuteDelegate = param => true
            };
        }

        internal static bool OpenURL(string url)
        {
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