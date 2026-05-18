using System;
using System.IO;
using Newtonsoft.Json;

namespace LicenseGenerator
{
    /// <summary>
    /// 授权文件生成器
    /// </summary>
    public class LicenseGenerator
    {
        // 用于生成授权的公钥（仅在生成授权时使用，不能包含在客户端程序中）
        private static readonly string PrivateKey = @"<RSAKeyValue><Modulus>3XE9NsLoFcuJV2NDtRd8Q9TIolEE1TWwGX0tAVYS+n3gLneJvKdzekBFfnS54W0vGXqivncBs5Nw18vWHKi/gVwDy/5QPwUMBrHtfp9Si1y4J8rDtPbuEyTsnSxNDFuypsoCQiKTx5CuPFX6XsEwJ4WfOiS5ydMZ+ZX6CYlEiLHJY+JCfnTIfceVv9GNmFufHs7UN6jrr6w7uAI9/36gVDq/y1HDRr0rMCuMbi/QlhsM9z9cZ8vGXZ6LAq9elBe08eJ2/ChkV5yhwDkF7GuiP4j+YbQYB4iYKTvA4A7ry3iVGtjS5V/35TUgTUNYRKPGKywnw5W0rW+F9yzEh8GKhQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// 生成授权文件
        /// </summary>
        /// <param name="licenseKey">授权码</param>
        /// <param name="expireDate">过期日期</param>
        /// <param name="outputPath">输出路径</param>
        /// <returns>是否生成成功</returns>
        public static bool GenerateLicenseFile(string licenseKey, string hardwareFingerprint, DateTime expireDate)
        {
            try
            {
                // 构造授权信息对象
                var licenseInfo = new LicenseInfo
                {
                    LicenseKey = licenseKey,
                    ExpireDate = expireDate,
                    HardwareFingerprint = hardwareFingerprint,
                };

                // 序列化授权信息
                string licenseData = JsonConvert.SerializeObject(licenseInfo);

                // 使用公钥加密授权数据
                string encryptedData = Rsa2Encryption.Encrypt(licenseData, PrivateKey);

                var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.txt");
                if (Directory.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
                // 写入授权文件
                File.WriteAllText(outputPath, encryptedData);

                return true;
            }
            catch (Exception ex)
            {
                // 在实际应用中，这里应该记录日志
                Console.WriteLine($"生成授权文件失败：{ex.Message}");
                return false;
            }
        }

    }
}