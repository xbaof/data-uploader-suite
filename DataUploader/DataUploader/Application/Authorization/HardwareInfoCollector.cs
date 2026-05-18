using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Management;

namespace DataUploader.Application.Authorization
{
    /// <summary>
    /// 硬件信息收集器，用于收集计算机唯一标识信息
    /// </summary>
    public class HardwareInfoCollector
    {
        /// <summary>
        /// 获取计算机硬件指纹
        /// </summary>
        /// <returns>硬件指纹字符串</returns>
        public static string GetHardwareFingerprint()
        {
            try
            {
                var hardwareInfo = new StringBuilder();

                // 获取CPU序列号
                hardwareInfo.Append(GetCpuId());

                // 获取主板序列号
                hardwareInfo.Append(GetMotherboardId());

                // 获取硬盘序列号
                hardwareInfo.Append(GetHardDiskId());

                // 获取BIOS序列号
                hardwareInfo.Append(GetBiosId());

                // 使用SHA256生成指纹摘要
                using (var sha256 = SHA256.Create())
                {
                    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(hardwareInfo.ToString()));
                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (Exception ex)
            {
                // 如果无法获取硬件信息，则使用机器名和用户名作为备用方案
                return GetFallbackFingerprint();
            }
        }

        /// <summary>
        /// 获取CPU ID
        /// </summary>
        private static string GetCpuId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["ProcessorId"]?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                // 忽略异常，返回空字符串
            }
            return "";
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        private static string GetMotherboardId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                // 忽略异常，返回空字符串
            }
            return "";
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        private static string GetHardDiskId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive WHERE MediaType LIKE '%Fixed%'"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                // 忽略异常，返回空字符串
            }
            return "";
        }

        /// <summary>
        /// 获取BIOS序列号
        /// </summary>
        private static string GetBiosId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                // 忽略异常，返回空字符串
            }
            return "";
        }

        /// <summary>
        /// 备用指纹生成方法（当无法获取硬件信息时使用）
        /// </summary>
        private static string GetFallbackFingerprint()
        {
            var fallbackInfo = $"{Environment.MachineName}-{Environment.UserName}";
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fallbackInfo));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
