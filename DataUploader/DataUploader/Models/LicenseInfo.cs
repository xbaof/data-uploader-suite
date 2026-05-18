using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUploader.Models
{
    [SugarTable("license_info")]
    public class LicenseInfo
    {
        /// <summary>
        /// 授权信息集
        /// </summary>
        [SugarColumn(ColumnName = "license_data")]
        public string LicenseData { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 硬件指纹
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string HardwareFingerprint { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string LicenseKey { get; set; }
    }
}
