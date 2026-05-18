using System;
using DataUploader.Application;

namespace DataUploader.Demo
{
    /// <summary>
    /// RSA2加密使用示例
    /// </summary>
    public class Rsa2UsageExample
    {
        /// <summary>
        /// 基本加解密示例
        /// </summary>
        public static void BasicEncryptionExample()
        {
            Console.WriteLine("=== RSA2 基本加解密示例 ===");

            // 生成密钥对
            var (publicKey, privateKey) = Rsa2Encryption.GenerateKeyPair();
            Console.WriteLine($"公钥: {publicKey}");
            Console.WriteLine($"私钥: {privateKey}");

            // 要加密的数据
            string originalData = "这是一段需要加密的敏感数据";
            Console.WriteLine($"原始数据: {originalData}");

            // 使用公钥加密
            string encryptedData = Rsa2Encryption.Encrypt(originalData, publicKey);
            Console.WriteLine($"加密后数据: {encryptedData}");

            // 使用私钥解密
            string decryptedData = Rsa2Encryption.Decrypt(encryptedData, privateKey);
            Console.WriteLine($"解密后数据: {decryptedData}");

            Console.WriteLine($"加解密是否成功: {originalData == decryptedData}");
            Console.WriteLine();
        }

        /// <summary>
        /// 综合使用示例
        /// </summary>
        public static void ComprehensiveExample()
        {
            Console.WriteLine("=== RSA2 综合使用示例 ===");

            // 生成密钥对
            var (publicKey, privateKey) = Rsa2Encryption.GenerateKeyPair();

            // 模拟业务数据
            string businessData = "用户ID:1001,操作:转账,金额:5000,时间:2023-10-28 10:30:00";
            Console.WriteLine($"业务数据: {businessData}");

            // 1. 加密敏感数据
            string encryptedData = Rsa2Encryption.Encrypt(businessData, publicKey);
            Console.WriteLine($"加密后数据: {encryptedData}");


            // 3. 传输加密数据和签名...

            // 4. 接收方解密数据
            string decryptedData = Rsa2Encryption.Decrypt(encryptedData, privateKey);
            Console.WriteLine($"解密后数据: {decryptedData}");


        }
    }
}