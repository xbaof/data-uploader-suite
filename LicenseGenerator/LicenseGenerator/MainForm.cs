using System;
using System.Windows.Forms;

namespace LicenseGenerator
{
    /// <summary>
    /// 授权文件生成演示
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void LicenseGeneratorDemo_Load(object sender, EventArgs e)
        {
            // 初始化界面
            txtHardwareFingerprint.Text = "";
            dtExpireDate.Value = DateTime.Now.AddYears(2);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string hardwareFingerprint = txtHardwareFingerprint.Text.Trim();
                DateTime expireDate = dtExpireDate.Value;

                if (string.IsNullOrEmpty(hardwareFingerprint))
                {
                    MessageBox.Show("请输入主板序列号。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 生成授权文件
                bool result = LicenseGenerator.GenerateLicenseFile("DataUploader2025", hardwareFingerprint, expireDate);

                if (result)
                {
                    MessageBox.Show("授权文件生成成功！文件位置：" +
                        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.txt"),
                        "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("授权文件生成失败，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成授权文件时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Windows Form Designer generated code

        private System.ComponentModel.IContainer components = null;
        private TextBox txtHardwareFingerprint;
        private DateTimePicker dtExpireDate;
        private Button btnGenerate;
        private Label lblLicenseKey;
        private TextBox txtLicenseData;
        private Button button1;
        private Label label1;
        private Label lblExpireDate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtHardwareFingerprint = new TextBox();
            dtExpireDate = new DateTimePicker();
            btnGenerate = new Button();
            lblLicenseKey = new Label();
            lblExpireDate = new Label();
            txtLicenseData = new TextBox();
            button1 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // txtHardwareFingerprint
            // 
            txtHardwareFingerprint.Location = new Point(106, 17);
            txtHardwareFingerprint.Name = "txtHardwareFingerprint";
            txtHardwareFingerprint.Size = new Size(270, 23);
            txtHardwareFingerprint.TabIndex = 0;
            // 
            // dtExpireDate
            // 
            dtExpireDate.Location = new Point(106, 60);
            dtExpireDate.Name = "dtExpireDate";
            dtExpireDate.Size = new Size(134, 23);
            dtExpireDate.TabIndex = 1;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(272, 58);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(104, 24);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "生成授权码";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // lblLicenseKey
            // 
            lblLicenseKey.AutoSize = true;
            lblLicenseKey.Location = new Point(20, 20);
            lblLicenseKey.Name = "lblLicenseKey";
            lblLicenseKey.Size = new Size(80, 17);
            lblLicenseKey.TabIndex = 4;
            lblLicenseKey.Text = "主板序列号：";
            lblLicenseKey.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblExpireDate
            // 
            lblExpireDate.AutoSize = true;
            lblExpireDate.Location = new Point(32, 65);
            lblExpireDate.Name = "lblExpireDate";
            lblExpireDate.Size = new Size(68, 17);
            lblExpireDate.TabIndex = 5;
            lblExpireDate.Text = "过期时间：";
            lblExpireDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtLicenseData
            // 
            txtLicenseData.Location = new Point(106, 108);
            txtLicenseData.Name = "txtLicenseData";
            txtLicenseData.Size = new Size(270, 23);
            txtLicenseData.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(291, 148);
            button1.Name = "button1";
            button1.Size = new Size(85, 24);
            button1.TabIndex = 7;
            button1.Text = "验证授权码";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 111);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 8;
            label1.Text = "授权码：";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LicenseGeneratorDemo
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(401, 198);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(txtLicenseData);
            Controls.Add(txtHardwareFingerprint);
            Controls.Add(dtExpireDate);
            Controls.Add(btnGenerate);
            Controls.Add(lblLicenseKey);
            Controls.Add(lblExpireDate);
            Name = "LicenseGeneratorDemo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "授权文件生成工具";
            Load += LicenseGeneratorDemo_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            string hardwareFingerprint = txtHardwareFingerprint.Text.Trim();
            string licenseData = txtLicenseData.Text.Trim();
            try
            {
               var result= LicenseValidator.VerifyLicenseData(licenseData, hardwareFingerprint);
                if (result.Item1)
                {
                    MessageBox.Show("验证成功！",
                        "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("验证失败，授权码无效！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("验证授权时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}