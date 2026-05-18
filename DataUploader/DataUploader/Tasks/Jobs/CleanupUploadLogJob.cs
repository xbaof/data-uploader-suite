using Quartz;
using DataUploader.Application;
using SqlSugar;
using DataUploader.Models;

namespace DataUploader.Tasks.Jobs
{
    /// <summary>
    /// 清理上传日志任务，删除60天前的数据
    /// </summary>
    public class CleanupUploadLogJob : JobBase
    {
        /// <summary>
        /// 执行清理上传日志任务
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        protected override async Task ExecuteJobAsync(IJobExecutionContext context)
        {
            // 连接数据库
            using var db = new SqlSugarOperation(GlobalConfig.UploaderDB, DbType.Sqlite);
            // 删除60天前的数据
            var deletedCount = await db.DeleteAsync<UploadLog>(x => x.RequestTime < DateTime.Now.AddDays(-60));
        }
    }
}