using AntdUI;
using System.Windows.Forms;

namespace DataUploader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Tabs.StyleCard styleCard1 = new Tabs.StyleCard();
            titlebar = new PageHeader();
            button_color = new AntdUI.Button();
            button_set = new AntdUI.Button();
            tabs = new Tabs();
            tabPage = new AntdUI.TabPage();
            logInput = new Input();
            tabPage1 = new AntdUI.TabPage();
            panelMain = new AntdUI.Panel();
            table1 = new Table();
            pagination1 = new Pagination();
            flowPanel1 = new FlowPanel();
            btnExport = new AntdUI.Button();
            btnSearch = new AntdUI.Button();
            inputKeyword = new Input();
            resultSelect = new Select();
            label2 = new AntdUI.Label();
            datePickerRange1 = new DatePickerRange();
            label1 = new AntdUI.Label();
            notifyIcon1 = new NotifyIcon(components);
            titlebar.SuspendLayout();
            tabs.SuspendLayout();
            tabPage.SuspendLayout();
            tabPage1.SuspendLayout();
            panelMain.SuspendLayout();
            flowPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // titlebar
            // 
            titlebar.Controls.Add(button_color);
            titlebar.Controls.Add(button_set);
            titlebar.DividerShow = true;
            titlebar.Dock = DockStyle.Top;
            titlebar.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            titlebar.Location = new Point(0, 0);
            titlebar.Name = "titlebar";
            titlebar.ShowButton = true;
            titlebar.ShowIcon = true;
            titlebar.Size = new Size(1024, 40);
            titlebar.SubText = "Demo";
            titlebar.TabIndex = 0;
            titlebar.Text = "中间同步程序";
            // 
            // button_color
            // 
            button_color.Dock = DockStyle.Right;
            button_color.Ghost = true;
            button_color.IconSvg = "SunOutlined";
            button_color.Location = new Point(780, 0);
            button_color.Name = "button_color";
            button_color.Radius = 0;
            button_color.Size = new Size(50, 40);
            button_color.TabIndex = 2;
            button_color.ToggleIconSvg = "MoonOutlined";
            button_color.WaveSize = 0;
            // 
            // button_set
            // 
            button_set.Dock = DockStyle.Right;
            button_set.Ghost = true;
            button_set.IconRatio = 0.8F;
            button_set.IconSvg = resources.GetString("button_set.IconSvg");
            button_set.Location = new Point(830, 0);
            button_set.Name = "button_set";
            button_set.Radius = 0;
            button_set.Size = new Size(50, 40);
            button_set.TabIndex = 1;
            button_set.ToggleIconSvg = "MoonOutlined";
            button_set.WaveSize = 0;
            // 
            // tabs
            // 
            tabs.Controls.Add(tabPage);
            tabs.Controls.Add(tabPage1);
            tabs.Dock = DockStyle.Fill;
            tabs.Gap = 18;
            tabs.Location = new Point(0, 40);
            tabs.Margin = new Padding(5, 3, 3, 3);
            tabs.Name = "tabs";
            tabs.Pages.Add(tabPage);
            tabs.Pages.Add(tabPage1);
            tabs.Size = new Size(1024, 600);
            tabs.Style = styleCard1;
            tabs.TabIndex = 9;
            tabs.Type = TabType.Card;
            // 
            // tabPage
            // 
            tabPage.Controls.Add(logInput);
            tabPage.IconSvg = "HomeOutlined";
            tabPage.Location = new Point(5, 37);
            tabPage.Name = "tabPage";
            tabPage.ReadOnly = true;
            tabPage.Size = new Size(1016, 560);
            tabPage.TabIndex = 1;
            tabPage.Text = "实时日志";
            // 
            // logInput
            // 
            logInput.AcceptsTab = true;
            logInput.AutoScroll = true;
            logInput.Dock = DockStyle.Fill;
            logInput.Location = new Point(0, 0);
            logInput.MaxLength = 299009;
            logInput.Multiline = true;
            logInput.Name = "logInput";
            logInput.Radius = 1;
            logInput.ReadOnly = true;
            logInput.Size = new Size(1016, 560);
            logInput.TabIndex = 0;
            logInput.MouseDown += LogInput_MouseDown;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panelMain);
            tabPage1.Location = new Point(-1024, -563);
            tabPage1.Name = "tabPage1";
            tabPage1.ReadOnly = true;
            tabPage1.Size = new Size(1024, 563);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "日志查询";
            // 
            // panelMain
            // 
            panelMain.Controls.Add(table1);
            panelMain.Controls.Add(pagination1);
            panelMain.Controls.Add(flowPanel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(3);
            panelMain.Size = new Size(1024, 563);
            panelMain.TabIndex = 0;
            // 
            // table1
            // 
            table1.AutoSizeColumnsMode = ColumnsMode.Fill;
            table1.Bordered = true;
            table1.Dock = DockStyle.Fill;
            table1.EmptyHeader = true;
            table1.Gap = 8;
            table1.Location = new Point(3, 39);
            table1.Name = "table1";
            table1.Padding = new Padding(3, 3, 3, 2);
            table1.Size = new Size(1018, 481);
            table1.TabIndex = 3;
            // 
            // pagination1
            // 
            pagination1.Dock = DockStyle.Bottom;
            pagination1.Location = new Point(3, 520);
            pagination1.Name = "pagination1";
            pagination1.Padding = new Padding(0, 0, 0, 4);
            pagination1.Radius = 3;
            pagination1.RightToLeft = RightToLeft.Yes;
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(1018, 40);
            pagination1.SizeChangerWidth = 85;
            pagination1.TabIndex = 2;
            pagination1.Text = "pagination1";
            // 
            // flowPanel1
            // 
            flowPanel1.Controls.Add(btnExport);
            flowPanel1.Controls.Add(btnSearch);
            flowPanel1.Controls.Add(inputKeyword);
            flowPanel1.Controls.Add(resultSelect);
            flowPanel1.Controls.Add(label2);
            flowPanel1.Controls.Add(datePickerRange1);
            flowPanel1.Controls.Add(label1);
            flowPanel1.Dock = DockStyle.Top;
            flowPanel1.Location = new Point(3, 3);
            flowPanel1.Name = "flowPanel1";
            flowPanel1.Size = new Size(1018, 36);
            flowPanel1.TabIndex = 0;
            flowPanel1.Text = "flowPanel1";
            // 
            // btnExport
            // 
            btnExport.IconRatio = 0.9F;
            btnExport.IconSvg = "DownloadOutlined";
            btnExport.Location = new Point(757, 4);
            btnExport.Margin = new Padding(4, 4, 3, 3);
            btnExport.Name = "btnExport";
            btnExport.Radius = 3;
            btnExport.Size = new Size(64, 28);
            btnExport.TabIndex = 25;
            btnExport.Text = "导出";
            btnExport.Type = TTypeMini.Primary;
            btnExport.WaveSize = 0;
            btnExport.Click += ExportDataButton_Clic;
            // 
            // btnSearch
            // 
            btnSearch.IconRatio = 0.9F;
            btnSearch.IconSvg = "SearchOutlined";
            btnSearch.Location = new Point(686, 4);
            btnSearch.Margin = new Padding(12, 4, 3, 3);
            btnSearch.Name = "btnSearch";
            btnSearch.Radius = 3;
            btnSearch.Size = new Size(64, 28);
            btnSearch.TabIndex = 24;
            btnSearch.Text = "查询";
            btnSearch.Type = TTypeMini.Primary;
            btnSearch.WaveSize = 0;
            btnSearch.Click += SearchButton_Click;
            // 
            // inputKeyword
            // 
            inputKeyword.AllowClear = true;
            inputKeyword.Location = new Point(411, 3);
            inputKeyword.Margin = new Padding(6, 3, 3, 3);
            inputKeyword.Name = "inputKeyword";
            inputKeyword.PlaceholderText = "请输入关键字进行查询";
            inputKeyword.PrefixFore = SystemColors.ActiveBorder;
            inputKeyword.PrefixSvg = "SearchOutlined";
            inputKeyword.Radius = 3;
            inputKeyword.Size = new Size(260, 30);
            inputKeyword.TabIndex = 23;
            inputKeyword.WaveSize = 0;
            // 
            // resultSelect
            // 
            resultSelect.AllowClear = true;
            resultSelect.Location = new Point(313, 3);
            resultSelect.Margin = new Padding(0, 3, 3, 3);
            resultSelect.Name = "resultSelect";
            resultSelect.Radius = 3;
            resultSelect.Size = new Size(89, 30);
            resultSelect.TabIndex = 22;
            resultSelect.WaveSize = 0;
            // 
            // label2
            // 
            label2.Location = new Point(269, 3);
            label2.Margin = new Padding(3, 3, 0, 3);
            label2.Name = "label2";
            label2.Size = new Size(44, 30);
            label2.TabIndex = 18;
            label2.Text = "说明：";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // datePickerRange1
            // 
            datePickerRange1.Location = new Point(49, 3);
            datePickerRange1.Margin = new Padding(0, 3, 3, 3);
            datePickerRange1.Name = "datePickerRange1";
            datePickerRange1.PlaceholderEnd = "结束日期";
            datePickerRange1.PlaceholderStart = "开始日期";
            datePickerRange1.Radius = 3;
            datePickerRange1.Size = new Size(214, 30);
            datePickerRange1.TabIndex = 3;
            datePickerRange1.WaveSize = 0;
            // 
            // label1
            // 
            label1.Location = new Point(3, 3);
            label1.Margin = new Padding(3, 3, 0, 3);
            label1.Name = "label1";
            label1.Size = new Size(46, 30);
            label1.TabIndex = 2;
            label1.Text = "时间：";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "中间同步程序";
            notifyIcon1.Visible = true;
            // 
            // MainForm
            // 
            ClientSize = new Size(1024, 640);
            ControlBox = false;
            Controls.Add(tabs);
            Controls.Add(titlebar);
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "中间同步程序";
            titlebar.ResumeLayout(false);
            tabs.ResumeLayout(false);
            tabPage.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            flowPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Button button_set;
        private AntdUI.Tabs tabs;
        private AntdUI.TabPage tabPage;
        private AntdUI.TabPage tabPage1;
        public AntdUI.Input logInput;
        private AntdUI.Panel panelMain;
        private AntdUI.FlowPanel flowPanel1;
        private AntdUI.Label label1;
        private DatePickerRange datePickerRange1;
        private AntdUI.Label label2;
        private AntdUI.Select resultSelect;
        private AntdUI.Button btnExport;
        private AntdUI.Button btnSearch;
        private Input inputKeyword;
        private Pagination pagination1;
        private Table table1;
        private NotifyIcon notifyIcon1;
        private AntdUI.Button button_color;
    }
}