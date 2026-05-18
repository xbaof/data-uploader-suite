using DataUploader.Application.Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DataUploader
{
    internal static class Program
    {
        private static MainForm mainForm;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var mutex = new Mutex(true, GlobalConfig.UniqueAppId, out bool createdNew))
            {
                if (createdNew)
                {
                    // 首次启动：正常运行程序
                    System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                    AntdUI.Localization.DefaultLanguage = "zh-CN";
                    //若文字不清晰，切换其他渲染方式
                    AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    AntdUI.Config.TextRenderingHighQuality = true;
                    
                    System.Windows.Forms.Application.EnableVisualStyles();
                    System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                    mainForm = new MainForm();
                    SerilogRegister.InitSerilog(mainForm.logInput);
                    System.Windows.Forms.Application.Run(mainForm);
                }
                else
                {
                    // 已存在实例：激活已有窗口
                    ActivateExistingInstance();
                }
            }
        }

        // 捕获UI线程中的未处理异常
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            AntdUI.Notification.error(mainForm, "未处理的UI线程异常", e.Exception.Message, autoClose: 3, align: AntdUI.TAlignFrom.TR);
        }

        // 捕获非UI线程中的未处理异常
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AntdUI.Notification.error(mainForm, "未处理的非UI线程异常", e.ToString(), autoClose: 3, align: AntdUI.TAlignFrom.TR);
        }

        #region 激活已运行的程序窗口
        private static void ActivateExistingInstance()
        {
            // 查找所有同名进程
            var currentProcess = Process.GetCurrentProcess();
            foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id) // 排除当前进程
                {
                    // 激活窗口（处理最小化状态）
                    IntPtr mainWindowHandle = process.MainWindowHandle;
                    if (mainWindowHandle != IntPtr.Zero)
                    {
                        // 先恢复窗口（如果最小化）
                        ShowWindowAsync(mainWindowHandle, SW_RESTORE);
                        // 再激活窗口
                        SetForegroundWindow(mainWindowHandle);
                        break;
                    }
                }
            }
        }
        // 导入 Windows API 用于窗口操作
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_RESTORE = 9; // 恢复窗口的命令
        #endregion
    }
}