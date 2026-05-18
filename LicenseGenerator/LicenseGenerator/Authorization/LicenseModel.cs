using System;

namespace LicenseGenerator
{
    public class LicenseInfo
    {
        /// <summary>
        /// 授权信息集
        /// </summary>
        public string LicenseData { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 硬件指纹
        /// </summary>
        public string HardwareFingerprint { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string LicenseKey { get; set; }
    }
}