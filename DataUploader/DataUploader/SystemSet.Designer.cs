namespace DataUploader
{
    partial class SystemSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            stackPanel1 = new AntdUI.StackPanel();
            flowLayoutPanel2 = new AntdUI.In.FlowLayoutPanel();
            label1 = new AntdUI.Label();
            input_LicenseData = new AntdUI.Input();
            flowLayoutPanel1 = new AntdUI.In.FlowLayoutPanel();
            label2 = new AntdUI.Label();
            input_HardwareFingerprint = new AntdUI.Input();
            stackPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // stackPanel1
            // 
            stackPanel1.Controls.Add(flowLayoutPanel2);
            stackPanel1.Controls.Add(flowLayoutPanel1);
            stackPanel1.Dock = DockStyle.Fill;
            stackPanel1.Location = new Point(0, 0);
            stackPanel1.Margin = new Padding(0);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new Size(544, 116);
            stackPanel1.TabIndex = 0;
            stackPanel1.Text = "stackPanel1";
            stackPanel1.Vertical = true;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(label1);
            flowLayoutPanel2.Controls.Add(input_LicenseData);
            flowLayoutPanel2.Location = new Point(3, 57);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(538, 36);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // label1
            // 
            label1.Font = new Font("Microsoft YaHei UI", 10F);
            label1.Location = new Point(3, 3);
            label1.Margin = new Padding(3, 3, 0, 3);
            label1.Name = "label1";
            label1.Size = new Size(81, 30);
            label1.TabIndex = 2;
            label1.Text = "授权码：";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // input_LicenseData
            // 
            input_LicenseData.Font = new Font("Microsoft YaHei UI", 9F);
            input_LicenseData.Location = new Point(84, 3);
            input_LicenseData.Margin = new Padding(0, 3, 3, 3);
            input_LicenseData.Name = "input_LicenseData";
            input_LicenseData.Radius = 3;
            input_LicenseData.Size = new Size(382, 30);
            input_LicenseData.TabIndex = 3;
            input_LicenseData.WaveSize = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(input_HardwareFingerprint);
            flowLayoutPanel1.Location = new Point(3, 15);
            flowLayoutPanel1.Margin = new Padding(3, 15, 3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(538, 36);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(3, 3);
            label2.Margin = new Padding(3, 3, 0, 3);
            label2.Name = "label2";
            label2.Size = new Size(81, 30);
            label2.TabIndex = 2;
            label2.Text = "主板序列号：";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // input_HardwareFingerprint
            // 
            input_HardwareFingerprint.Font = new Font("Microsoft YaHei UI", 9F);
            input_HardwareFingerprint.Location = new Point(84, 3);
            input_HardwareFingerprint.Margin = new Padding(0, 3, 3, 3);
            input_HardwareFingerprint.Name = "input_HardwareFingerprint";
            input_HardwareFingerprint.Radius = 3;
            input_HardwareFingerprint.ReadOnly = true;
            input_HardwareFingerprint.Size = new Size(382, 30);
            input_HardwareFingerprint.TabIndex = 3;
            input_HardwareFingerprint.WaveSize = 0;
            // 
            // SystemSet
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            Controls.Add(stackPanel1);
            Name = "SystemSet";
            Size = new Size(544, 116);
            stackPanel1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.StackPanel stackPanel1;
        private AntdUI.In.FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Label label2;
        private AntdUI.Input input_HardwareFingerprint;
        private AntdUI.In.FlowLayoutPanel flowLayoutPanel2;
        private AntdUI.Label label1;
        private AntdUI.Input input_LicenseData;
    }
}
