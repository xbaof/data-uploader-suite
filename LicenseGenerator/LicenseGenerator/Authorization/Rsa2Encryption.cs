using System;
using System.Security.Cryptography;
using System.Text;

namespace LicenseGenerator
{
    /// <summary>
    /// RSA2加密工具类
    /// </summary>
    public class Rsa2Encryption
    {
        /// <summary>
        /// 生成RSA密钥对
        /// </summary>
        /// <param name="keySize">密钥长度，默认2048</param>
        /// <returns>包含公钥和私钥的元组</returns>
        public static (string publicKey, string privateKey) GenerateKeyPair(int keySize = 2048)
        {
            using var rsa = RSA.Create();
            rsa.KeySize = keySize;
            
            var publicKey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);
            
            return (publicKey, privateKey);
        }
        
        /// <summary>
        /// 使用公钥加密数据
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string Encrypt(string data, string publicKey)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("数据不能为空", nameof(data));
                
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentException("公钥不能为空", nameof(publicKey));
                
            using var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);
            
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var encryptedBytes = rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);
            
            return Convert.ToBase64String(encryptedBytes);
        }
        
        /// <summary>
        /// 使用私钥解密数据
        /// </summary>
        /// <param name="encryptedData">加密后的Base64字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>解密后的原始数据</returns>
        public static string Decrypt(string encryptedData, string privateKey)
        {
            if (string.IsNullOrEmpty(encryptedData))
                throw new ArgumentException("加密数据不能为空", nameof(encryptedData));
                
            if (string.IsNullOrEmpty(privateKey))
                throw new ArgumentException("私钥不能为空", nameof(privateKey));
                
            using var rsa = RSA.Create();
            try
            {
                rsa.FromXmlString(privateKey);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("无法加载私钥，密钥格式可能不正确", ex);
            }
            
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedData);
                var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (CryptographicException)
            {
                // 尝试使用不同的填充方式
                try
                {
                    var encryptedBytes = Convert.FromBase64String(encryptedData);
                    var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
                catch (Exception ex)
                {
                    throw new CryptographicException("解密失败，请检查密钥和数据是否匹配且有效", ex);
                }
            }
        }
        
    }
}