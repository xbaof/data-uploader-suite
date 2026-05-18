using Serilog.Events;
using Serilog;
using System.Text;
using Serilog.Filters;
using AntdUI;

namespace DataUploader.Application.Serilog
{
    public class SerilogRegister
    {
        /// <summary>
        /// 添加默认日志拓展
        /// </summary>
        public static void InitSerilog(Input logInput)
        {
            string outputTemplate = "【{Level:u3}】{Timestamp:yyyy-MM-dd HH:mm:ss.fff}" +
            "{NewLine}#Msg# {Message:lj}" +
            "{NewLine}#Pro# {Properties:j}" +
            "{NewLine}#Exc# {Exception}" +
            new string('-', 50) + "{NewLine}";//输出模板

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()//最小记录级别
                                       //对其他日志进行重写
                .MinimumLevel.Override("Quartz", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)

                .Enrich.FromLogContext()//记录相关上下文信息 
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level <= LogEventLevel.Fatal && evt.Level >= LogEventLevel.Error)//筛选过滤
                .WriteTo.File($"Logs/ErrorLog/errorLogs_.log",
                    outputTemplate: outputTemplate,
                    rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
                    retainedFileCountLimit: 7,
                    encoding: Encoding.UTF8))

                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level <= LogEventLevel.Warning && evt.Level >= LogEventLevel.Debug)//筛选过滤
                .WriteTo.File($"Logs/InfoLog/infoLogs_.log",
                    outputTemplate: outputTemplate,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 3,
                    encoding: Encoding.UTF8))

                // 添加上传日志专用文件
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(Matching.FromSource("DataUploader.Tasks.UploadLog"))
                .WriteTo.File(formatter: new SerilogJsonFormatter(),$"Logs/UploadLog/uploadLogs_.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7, // 保留近7天的上传日志
                    encoding: Encoding.UTF8))

                .WriteTo.Logger(lg => lg.Filter.ByExcluding(Matching.FromSource("DataUploader.Tasks.UploadLog"))
                  .WriteTo.Sink(new WinFormsLogSink(logInput)))
                .CreateLogger();
        }
     
    }
}
