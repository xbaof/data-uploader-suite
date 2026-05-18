using Quartz;

namespace DataUploader.Tasks
{
    /// <summary>
    /// 任务基类，提供通用的任务执行逻辑和日志记录
    /// </summary>
    public abstract class JobBase : IJob
    {
        public static readonly Serilog.ILogger _log = Serilog.Log.ForContext(typeof(JobBase));
        public static readonly Serilog.ILogger _uploadLog = Serilog.Log.ForContext("SourceContext", "DataUploader.Tasks.UploadLog");

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        public async Task Execute(IJobExecutionContext context)
        {
            var jobDescription = context.JobDetail.Description ?? "未命名任务";

            _log.Information($"任务【{jobDescription}】开始执行");

            try
            {
                // 记录任务开始时间
                var startTime = DateTime.Now;
                
                // 执行具体任务逻辑
                await ExecuteJobAsync(context);
                
                // 记录任务完成时间
                var endTime = DateTime.Now;
                var duration = endTime - startTime;
                
                _log.Information($"任务【{jobDescription}】执行完成 (耗时: {duration.TotalMilliseconds:F2}ms)");
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"任务【{jobDescription}】执行失败 ");
            }
        }

        /// <summary>
        /// 执行具体任务逻辑，由子类实现
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        /// <returns></returns>
        protected abstract Task ExecuteJobAsync(IJobExecutionContext context);
    }
}