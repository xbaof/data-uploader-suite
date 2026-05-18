using System;
using AntdUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataUploader.Application.Authorization;
using DataUploader.Application;
using DataUploader.Models;

namespace DataUploader
{
    public partial class SystemSet : UserControl
    {
        public string Input_HardwareFingerprint
        {
            get
            {
                return input_HardwareFingerprint.Text;
            }
        }
        public string Input_LicenseData
        {
            get
            {
                return input_LicenseData.Text;
            }
        }
        public SystemSet(Window _window)
        {
            InitializeComponent();
            //设置默认值
            InitData();
        }
        private async void InitData()
        {
            input_HardwareFingerprint.Text = HardwareInfoCollector.GetHardwareFingerprint();
            // 连接数据库
            using var db = new SqlSugarOperation(GlobalConfig.UploaderDB, SqlSugar.DbType.Sqlite);
            var info = await db.GetListAsync<LicenseInfo>();
            if (info.Count > 0)
            {
                input_LicenseData.Text = info[0].LicenseData;
            }
        }
    }
}
