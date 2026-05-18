using AntdUI;
using DataUploader;
using DataUploader.Application;
using DataUploader.Application.Utils;
using DataUploader.Models;
using Microsoft.Win32;
using SqlSugar;
using System.ComponentModel;
using Column = AntdUI.Column;
using System.Linq.Expressions;
using SqlSugar.Extensions;
using FolderBrowserDialog = AntdUI.FolderBrowserDialog;
using DataUploader.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DataUploader.Application.Authorization;

namespace DataUploader
{
    public partial class MainForm : AntdUI.Window
    {
        private bool isLight = true;
        private bool isLicense = false;
        private Form? menuStripForm = null;
        private CancellationTokenSource cancellationTokenSource;
        public static readonly Serilog.ILogger _log = Serilog.Log.ForContext(typeof(MainForm));

        // 定义一个语言切换事件
        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            //根据系统亮暗初始化一次
            isLight = ThemeHelper.IsLightMode();
            button_color.Toggle = !isLight;
            ThemeHelper.SetColorMode(this, isLight);
            //初始化消息弹出位置
            Config.ShowInWindow = true;

            menuStripForm = null;

            var today = DateTime.Now.Date;
            datePickerRange1.Value = [today.AddDays(-3), today];
            resultSelect.Items = ["成功", "失败"];

            table1.Columns = new ColumnCollection() {
                new Column("","序号",ColumnAlign.Center){
                    Width = "50",
                    Render = (value,record,rowindex)=>{return (rowindex+1); },
                    Fixed = true,//冻结列
                },
                new Column("ApiName", "接口名称",ColumnAlign.Center){ Fixed=true },
                new Column("ApiUrl", "接口地址",ColumnAlign.Center),
                new Column("RequestMethod", "请求方式",ColumnAlign.Center),
                new Column("RequestTimeFormat", "时间",ColumnAlign.Center),
                new Column("Note", "结果", ColumnAlign.Center),
                new Column("Duration", "耗时(毫秒)",ColumnAlign.Center),
                new Column("RequestJson", "请求",ColumnAlign.Center),
                new Column("ResponseJson", "响应", ColumnAlign.Center)
            };

            pagination1.PageSizeOptions = [20, 30, 40, 50];
            pagination1.PageSize = 30;
            pagination1.Current = 1;

            // 初始化取消令牌
            cancellationTokenSource = new CancellationTokenSource();

            // 绑定事件
            Load += MainForm_Load;
            button_set.Click += Button_setting_Click;
            button_color.Click += Button_color_Click;
            //监听系统深浅色变化
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            pagination1.ValueChanged += Pagination1_ValueChanged;
            // 绑定窗体关闭事件
            FormClosing += MainForm_FormClosing;
            // 绑定托盘图标点击事件
            notifyIcon1.MouseClick += NotifyIcon1_MouseClick;
        }

        #region 查询;导出
        private async Task BindTableData(bool showLoading = true)
        {
            if (showLoading)
            {
                PagedList<UploadLog> paged = new();
                await AntdUI.Spin.open(table1, new AntdUI.Spin.Config()
                {
                    Color = Style.Db.Primary,//转圈颜色
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 12f),//字体可以控制进度圈的大小

                }, async (config) =>
                {
                    paged = await GetPagedListAsync();
                }, () =>
                {
                    table1.Binding(new BindingList<UploadLog>(paged.Items));
                    pagination1.Total = paged.Total;
                    pagination1.TextDesc = $"共 {paged.Total} 条";
                });
            }
            else
            {
                var paged = await GetPagedListAsync();
                table1.Binding(new BindingList<UploadLog>(paged.Items));
                pagination1.Total = paged.Total;
                pagination1.TextDesc = $"共 {paged.Total} 条";
            }
        }

        public async Task<PagedList<UploadLog>> GetPagedListAsync()
        {
            var dates = datePickerRange1.Value;
            Expression<Func<UploadLog, bool>> expression = o => true;
            if (dates != null)
            {
                expression = expression.And(o => o.RequestTime >= dates[0] && o.RequestTime <= dates[1].AddHours(23).AddMinutes(59).AddSeconds(59));
            }
            if (!string.IsNullOrEmpty(resultSelect.SelectedValue.ObjToString()))
            {
                expression = expression.And(o => o.Result == (resultSelect.SelectedValue.ObjToString() == "成功"));
            }
            if (!string.IsNullOrEmpty(inputKeyword.Text))
            {
                expression = expression.And(o => o.ApiName.Contains(inputKeyword.Text) || o.ApiUrl.Contains(inputKeyword.Text) || o.RequestMethod.Contains(inputKeyword.Text));
            }

            using var operation = new SqlSugarOperation(GlobalConfig.UploaderDB, DbType.Sqlite);
            var pageIndex = pagination1.Current;
            var pageSize = pagination1.PageSize;
            return await operation.GetPagedListAsync<UploadLog>(expression, pageIndex, pageSize, " request_time desc");
        }

        private async void ExportDataButton_Clic(object sender, EventArgs e)
        {
            var dates = datePickerRange1.Value;
            if (dates == null)
            {
                AntdUI.Message.warn(this, "请先选择查询时间！", autoClose: 3);
                return;
            }
            TimeSpan duration = dates[1] - dates[0];
            if (duration.Days > 60)
            {
                AntdUI.Message.warn(this, "不能导出超过60天的记录！", autoClose: 3);
                return;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                btnExport.Loading = true;
                try
                {
                    await Task.Run(async () =>
                    {
                        Expression<Func<UploadLog, bool>> expression = o => true;
                        if (dates != null)
                        {
                            expression = expression.And(o => o.RequestTime >= dates[0] && o.RequestTime <= dates[1].AddHours(23).AddMinutes(59).AddSeconds(59));
                        }
                        if (!string.IsNullOrEmpty(resultSelect.SelectedValue.ObjToString()))
                        {
                            expression = expression.And(o => o.Result == (resultSelect.SelectedValue.ObjToString() == "成功"));
                        }
                        if (!string.IsNullOrEmpty(inputKeyword.Text))
                        {
                            expression = expression.And(o => o.ApiName.Contains(inputKeyword.Text) || o.ApiUrl.Contains(inputKeyword.Text) || o.RequestMethod.Contains(inputKeyword.Text));
                        }

                        using var operation = new SqlSugarOperation(GlobalConfig.UploaderDB, DbType.Sqlite);
                        var list = await operation.GetListAsync<UploadLog>(expression, " request_time desc");
                        string fileName = $"推送日志_{dates[0]:yyyy-MM-dd}_{dates[1]:yyyy-MM-dd}.xlsx";
                        var path = Path.Combine(folderBrowserDialog.DirectoryPath, fileName);
                        var mapper = new Npoi.Mapper.Mapper();
                        mapper.Save<UploadLog>(path, list, "sheet1", false);
                        btnExport.Loading = false;
                        AntdUI.Message.success(this, "导出成功！", autoClose: 3);
                    }, cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消，不处理
                }
                catch (Exception ex)
                {
                    btnExport.Loading = false;
                    AntdUI.Message.error(this, "导出失败：" + ex.Message, autoClose: 3);
                }
            }
        }
        #endregion

        #region 事件处理
        private void MainForm_Load(object sender, EventArgs e)
        {
            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 12f),//字体可以控制进度圈的大小
            }, async (config) =>
            {
                config.Text = "加载中...";

                GlobalConfig.LoadFromJson(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs\\config.json")));

                using var db = new SqlSugarOperation(GlobalConfig.UploaderDB, SqlSugar.DbType.Sqlite);
                var info = await db.GetListAsync<LicenseInfo>();
                var licenseData = info.Count > 0 ? info[0].LicenseData : "";
                var verify = LicenseValidator.VerifyLicenseData(licenseData);
                isLicense = verify.Item1;
                if (isLicense)
                {
                    // 初始化任务调度器
                    await SchedulerRegister.InitScheduler();
                }
                else
                {
                    _log.Warning(verify.Item2);
                    tabs.Invoke((MethodInvoker)(() =>
                    {
                        tabs.Enabled = false;
                    }));
                }

                //耗时代码，处理数据
                await Task.Delay(800);
            }, async () =>
            {
                await BindTableData(false);

            });
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                isLight = ThemeHelper.IsLightMode();
                button_color.Toggle = !isLight;
                ThemeHelper.SetColorMode(this, isLight);
            }
        }

        private void Button_color_Click(object sender, EventArgs e)
        {
            isLight = !isLight;
            //这里使用了Toggle属性切换图标
            button_color.Toggle = !isLight;
            ThemeHelper.SetColorMode(this, isLight);
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            pagination1.Current = 1;
            await BindTableData();
        }

        private async void Pagination1_ValueChanged(object sender, PagePageEventArgs e)
        {
            await BindTableData();
        }

        public void LogInput_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var svg_clear = "<svg t=\"1761976248452\" class=\"icon\" viewBox=\"0 0 1024 1024\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" p-id=\"5516\" width=\"16\" height=\"16\"><path d=\"M947.07772682 366.16525749l-83.09479349-40.89931852-84.33876385-43.72100741 104.67555555-204.77330014c7.05968355-13.72615111-3.6166163-30.72910222-18.19109451-37.84704-14.57326459-7.17376475-34.9100563-4.74529185-41.91391289 9.03789984l-104.61730133 204.65921896-78.68931793-37.22566163-107.78123378-53.04289659-3.21975941-1.3556243-3.33262696-1.12988918-3.38966755-0.67841896a68.57735585 68.57735585 0 0 0-3.44670815-0.45147022h-3.38845393l-3.38966756 0.16990815-3.38845392 0.56433777-3.2768 0.90415408-3.10689186 1.12867555a157.2657683 157.2657683 0 0 1-3.05106488 1.46849186l-2.87994312 1.80830814-2.71246222 2.03404326a43.0450157 43.0450157 0 0 0-2.54134044 2.25977837c-1.52553245 1.75126755-1.9782163 2.14569718-2.25977837 2.5971674a45.19071289 45.19071289 0 0 0-2.08987022 2.82532978l-1.69422697 2.88115675a673.79988859 673.79988859 0 0 1-96.59642311 137.83191704c-26.2677997 28.98026192-53.43853985 54.90824533-83.0960071 79.42477748-77.27665303 63.832064-167.65807882 115.23656059-268.66119112 152.68917095a34.45858608 34.45858608 0 0 0-22.59535645 34.74136178 34.28867792 34.28867792 0 0 0 16.777216 31.80316445l606.23955438 362.8279277a35.75959703 35.75959703 0 0 0 13.89605926 4.80111882 35.70134282 35.70134282 0 0 0 44.90915081-3.61418903C782.12968297 895.9752723 849.01281185 804.01127349 895.78609778 704.53248c47.3376237-100.54921482 71.51433955-203.36063525 71.90998281-305.60407703a33.89303467 33.89303467 0 0 0-6.26961067-19.77245393 35.13579141 35.13579141 0 0 0-14.3487431-12.99190519zM839.91787141 676.40054519c-41.23670755 87.72843141-99.25063111 169.46760059-172.40337066 243.41162666L529.39859437 837.17006222l86.76723674-83.0960071a35.02292385 35.02292385 0 0 0 0.56433778-50.331648 36.831232 36.831232 0 0 0-51.40449659-0.56433778l-99.87200948 95.69226903-162.46253037-97.21780148 98.06491496-67.33459911a35.07996445 35.07996445 0 0 0 8.92503229-49.59740208 36.831232 36.831232 0 0 0-50.6144237-8.69929718l-123.99289837 85.12883674-126.53545245-75.69408a1007.59294103 1007.59294103 0 0 0 236.01091319-144.44255762c25.75928889-21.35381333 49.93600475-43.61056711 72.41849363-66.82730194l445.30255645 249.0610726c-6.83516208 17.96171852-14.40457008 35.70012918-22.59535645 53.15455052zM452.51758459 319.90048237c24.74226725-30.67327525 46.99780741-62.70217482 66.54331259-95.91800415L906.01092741 414.46286222c-1.75126755 48.80611555-9.32067555 97.7263123-22.36962133 146.58946845l-431.12372149-241.20888889z\" p-id=\"5517\"></path></svg>";
                var menulist = new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("清屏")
                    {
                        IconSvg = svg_clear,
                    }
                };
                new AntdUI.ContextMenuStrip.Config(logInput, it =>
                {
                    logInput.Text = string.Empty;
                    logInput.ClearStyle();
                }, menulist, 0).open();
            }
        }

        // 托盘图标点击事件
        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 左键点击显示窗口
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
                ShowInTaskbar = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                menuStripForm?.Close();
                // 右键点击显示菜单
                var menulist = new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("显示窗口")
                    {
                        ID = "show",
                    },
                    new AntdUI.ContextMenuStripItem("退出")
                    {
                        ID = "exit",
                    }
                };
                menuStripForm = new AntdUI.ContextMenuStrip.Config(this, it =>
                {
                    if (it.ID == "show")
                    {
                        // 显示窗口
                        Show();
                        WindowState = FormWindowState.Normal;
                        Activate();
                        ShowInTaskbar = true;
                    }
                    else
                    {
                        // 退出程序
                        System.Windows.Forms.Application.Exit();
                    }
                }, menulist, 0).open();
                menuStripForm.TopMost = true;

            }
        }

        // 窗体关闭事件处理
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果是用户主动关闭窗口，则隐藏到托盘
            if (isLicense && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // 取消关闭操作
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
                Hide(); // 隐藏窗口
                notifyIcon1.ShowBalloonTip(2000, "提示", "程序已最小化到托盘，点击托盘图标可恢复窗口", ToolTipIcon.Info);
            }
        }

        private void Button_setting_Click(object? sender, EventArgs e)
        {
            using (var form = new SystemSet(this))
            {
                string title = AntdUI.Localization.Get("systemset", "更新授权");
                AntdUI.Modal.open(new AntdUI.Modal.Config(this, title, form, TType.Info)
                {
                    CloseIcon = true,
                    OnOk = config =>
                    {
                        var licenseData = form.Input_LicenseData;
                        if (!string.IsNullOrEmpty(licenseData))
                        {
                            var verify = LicenseValidator.VerifyLicenseData(licenseData);
                            if (verify.Item1)
                            {
                                using var db = new SqlSugarOperation(GlobalConfig.UploaderDB, SqlSugar.DbType.Sqlite);
                                try
                                {
                                    db.BeginTran();
                                    db.Delete<LicenseInfo>(o => true);
                                    db.Insert<LicenseInfo>(new LicenseInfo() { LicenseData = licenseData });
                                    db.CommitTran();
                                }
                                catch (Exception ex)
                                {
                                    db.RollbackTran();
                                    _log.Error($"保存授权信息失败，{ex.Message}");
                                }
                                if (!isLicense)
                                {
                                    // 初始化任务调度器
                                    _ = SchedulerRegister.InitScheduler();
                                    tabs.Invoke((MethodInvoker)(() =>
                                    {
                                        tabs.Enabled = true;
                                    }));
                                }
                                AntdUI.Message.success(this, verify.Item2, autoClose: 3);
                                isLicense = true;
                                return true;
                            }
                            else
                            {
                                AntdUI.Message.warn(this, verify.Item2, autoClose: 3);
                                return false;
                            }
                        }
                        return true;
                    },
                });
            }
        }

        #endregion

        #region 资源清理
        /// <summary>
        /// 释放窗体资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override async void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 取消所有正在进行的操作
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                await SchedulerRegister.ShutdownScheduler();
                // 取消事件订阅
                SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;

                // 释放菜单窗体
                menuStripForm?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

    }
}