using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LicenseGenerator
{
    /// <summary>
    /// 授权验证器
    /// </summary>
    public class LicenseValidator
    {

        // 用于验证授权的私钥（实际项目中应该从安全的地方获取）
        private static readonly string PublicKey = @"<RSAKeyValue><Modulus>3XE9NsLoFcuJV2NDtRd8Q9TIolEE1TWwGX0tAVYS+n3gLneJvKdzekBFfnS54W0vGXqivncBs5Nw18vWHKi/gVwDy/5QPwUMBrHtfp9Si1y4J8rDtPbuEyTsnSxNDFuypsoCQiKTx5CuPFX6XsEwJ4WfOiS5ydMZ+ZX6CYlEiLHJY+JCfnTIfceVv9GNmFufHs7UN6jrr6w7uAI9/36gVDq/y1HDRr0rMCuMbi/QlhsM9z9cZ8vGXZ6LAq9elBe08eJ2/ChkV5yhwDkF7GuiP4j+YbQYB4iYKTvA4A7ry3iVGtjS5V/35TUgTUNYRKPGKywnw5W0rW+F9yzEh8GKhQ==</Modulus><Exponent>AQAB</Exponent><P>9fkajGXhsQJsF/q1rfAYzD+k1r/wtMonK69jMlkc3DvI5j/2Snye3nZO+v5xsEoRKUr9Zb3TlvfV/QFWVbEmtA+b7fZdR7nSF8R8NSOZ2jQF8waQuIkTN0R6C3+V+SKvyziZFkTMYBrH0EhKyZRh9wt+0T6GjiinY5GXwWQ1lWs=</P><Q>5ngkATHOGprHFv4uSnnp/xZrvnbE02VcoznBhOuSTcobfCyjlE5kI2cLnkbJx+FbUrjKxl/YpjNs+sv9+87DBEu9mUctAIinywOM5+MmkcysfNq2C2BeJBmiEAZzPhI8KF8Pae3iQeCVFlF+Amt1VcDLJpwZqrBY+459FnVua88=</Q><DP>K8QYGjUilElXDGk6nGllMCp+3Qsn/DMgByDc6J98iB9HHtjwrM5BwlmQsg5GElULeqpyLgE5vdn5RFxRjUvJFC7W0t+MN2/z1vDKHPZpsK6jBFv9sigJuELB6HvaJosqdmFqs9CoAM2jEgda70UsrTVpajfS30aSih/kva9j7WM=</DP><DQ>PG0di46iaklQj8+/FVnXp0EaqMp9GqwF2OoIujAFoQ2vnSQbE4RmLv4ShsIqn5SPnCXFTLPOmuz60trvUblJWnP9VxXZT72EuO7KX/Yno6yVQPsbJepzZGWwHyGBz7F6Ql+CLBTp8LnQfRSKxFmmXwB4gzwEYsNv4Szf0Sai9/M=</DQ><InverseQ>O1MgHgqNialU0FxHohpCX0Vn7TsUGUlIVoBkgISCyRaqjRTiVD00SMoExlEr8E1TIH/WGpfkrcIG07+Pk7iGEASx/gTIBPyofX/UflzBzzfFsXBqjurq9tY0+JxpMab6WBb07Z53YcG0XKjlDwNaLwxznNSFmpfRIafBpuYhB9k=</InverseQ><D>iycgv2vFagTIHnkLUB4dAYcSOSXOcqjVK39Eq8VKEczN9oU5sapMX7NJuEhjkmwJH+q3oBaqpAzv6Tt3JfVIumJ3JFaAWMm0H/TpeBtoRJkURDUfvHU7cB5Q7gertytcXYEt4B9YXBzn0JR6y9Z8iyllwKTZdyFH/lYnlJyEy5w5utPy1t1Mx1eaKkGb8E7zbv9WGiPYD+3XSjtc8cFy55FQBThrxPBcDWWul7rMYOWnwsbQOpQMo+911x6LXnXzdyk/9rnmjcWS2eVJZiGr6drsG3oYVPxnQIF3Z9WUANMIn6bbIN1dTqCIyeclhAo17tEWKD8y/HCIqLupCFUZNQ==</D></RSAKeyValue>";

        /// <summary>
        /// 验证授权数据
        /// </summary>
        /// <param name="encryptedLicenseData">加密的授权数据</param>
        /// <returns>是否验证成功</returns>
        public static (bool,string) VerifyLicenseData(string encryptedLicenseData,string val)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedLicenseData))
                {
                    return (false, "未找到授权信息，请联系管理员获取授权。");
                }
                // 使用私钥解密授权数据
                string decryptedData = Rsa2Encryption.Decrypt(encryptedLicenseData, PublicKey);

                // 反序列化授权信息
                var licenseInfo = JsonConvert.DeserializeObject<LicenseInfo>(decryptedData);

                // 验证授权是否过期
                if (DateTime.Now > licenseInfo.ExpireDate)
                {
                    return (false, "授权已过期，请联系管理员续期。");
                }

                // 验证授权码
                if (licenseInfo.LicenseKey != "DataUploader2025")
                {
                    return (false, "授权码不正确。");
                }

                // 验证硬件指纹是否匹配
                if (licenseInfo.HardwareFingerprint != val)
                {
                    return (false, "授权码与当前计算机不匹配。");
                }

                return (true, "更新成功！");
            }
            catch (CryptographicException)
            {
                return (false, "授权码不正确！");
            }
            catch (Exception ex)
            {
                return (false, $"授权验证出错：{ex.Message}");
            }
        }

    }
}
