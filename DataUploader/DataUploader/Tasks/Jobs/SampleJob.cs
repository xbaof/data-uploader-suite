using Quartz;

namespace DataUploader.Tasks.Jobs
{
    /// <summary>
    /// 示例任务类，演示如何继承JobBase创建具体任务
    /// </summary>
    public class SampleJob : JobBase
    {
        /// <summary>
        /// 执行具体任务逻辑
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        protected override async Task ExecuteJobAsync(IJobExecutionContext context)
        {
            // 获取任务参数
            var jobParam = context.JobDetail.JobDataMap.GetString("JobParam");
            var jobId = context.JobDetail.Key.Name;
            
            
            // 模拟任务执行逻辑
            // 这里可以添加实际的业务逻辑，例如：
            // 1. 数据库操作
            // 2. API调用
            // 3. 文件处理
            // 4. 数据同步等
            
            // 模拟耗时操作
            await Task.Delay(2000);

            _uploadLog.Warning("执行具体任务逻辑");
        }
    }
}