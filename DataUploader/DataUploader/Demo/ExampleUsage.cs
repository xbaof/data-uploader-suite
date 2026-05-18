using DataUploader.Application;
using DataUploader.Models;
using SqlSugar;

namespace DataUploader.Demo
{
    public class ExampleUsage
    {
        private readonly string _connectionString = "DataSource=./DataBase/DataUploader.db";

        /// <summary>
        /// 示例：插入上传日志记录
        /// </summary>
        public async Task InsertUploadLogExample()
        {
            using var db = new SqlSugarOperation(_connectionString, DbType.Sqlite);

            var log = new UploadLog
            {
                ApiName = "用户登录接口",
                ApiUrl = "/api/login",
                RequestMethod = "POST",
                RequestTime = DateTime.Now,
                Duration = 150,
                RequestJson = "{\"username\":\"test\",\"password\":\"****\"}",
                ResponseJson = "{\"success\":true,\"token\":\"xxxxxx\"}",
                Result = true
            };

            // 插入单条记录
            var insertedLog = await db.InsertSync(log);
            Console.WriteLine($"插入记录ID: {insertedLog.Id}");
        }

        /// <summary>
        /// 示例：批量插入上传日志记录
        /// </summary>
        public async Task BatchInsertLogsExample()
        {
            using var db = new SqlSugarOperation(_connectionString, DbType.Sqlite);

            var logs = new List<UploadLog>
            {
                new UploadLog
                {
                    ApiName = "获取用户信息",
                    ApiUrl = "/api/user/info",
                    RequestMethod = "GET",
                    RequestTime = DateTime.Now.AddMinutes(-10),
                    Duration = 80,
                    RequestJson = "",
                    ResponseJson = "{\"userId\":1,\"name\":\"张三\"}",
                    Result = true
                },
                new UploadLog
                {
                    ApiName = "更新用户信息",
                    ApiUrl = "/api/user/update",
                    RequestMethod = "PUT",
                    RequestTime = DateTime.Now.AddMinutes(-5),
                    Duration = 120,
                    RequestJson = "{\"name\":\"李四\"}",
                    ResponseJson = "{\"success\":true}",
                    Result =true
                }
            };

            // 批量插入
            var result = await db.InsertListAsync(logs);
            Console.WriteLine($"批量插入了 {result} 条记录");
        }

        /// <summary>
        /// 示例：查询上传日志记录
        /// </summary>
        public async Task QueryUploadLogsExample()
        {
            using var db = new SqlSugarOperation(_connectionString, DbType.Sqlite);

            // 根据ID查询
            var logById = await db.GetById<UploadLog>(1);
            if (logById != null)
            {
                Console.WriteLine($"根据ID查询: {logById.ApiName}");
            }

            // 查询所有记录
            var allLogs = db.GetList<UploadLog>();
            Console.WriteLine($"总共 {allLogs.Count} 条记录");

            // 条件查询
            var postLogs = await db.GetListAsync<UploadLog>(
                l => l.RequestMethod == "POST",
                "RequestTime DESC"
            );
            Console.WriteLine($"POST请求有 {postLogs.Count} 条记录");

            // 分页查询
            var pagedLogs = db.GetPagedList<UploadLog>(
                l => l.RequestTime >= DateTime.Today.AddDays(-7),
                1, 10, "RequestTime DESC"
            );
            Console.WriteLine($"最近7天记录，第1页共{pagedLogs.Total}条");

            foreach (var log in pagedLogs.Items)
            {
                Console.WriteLine($"  {log.RequestTime:yyyy-MM-dd HH:mm:ss} - {log.ApiName}");
            }
        }

        /// <summary>
        /// 示例：更新上传日志记录
        /// </summary>
        public async Task UpdateUploadLogExample()
        {
            using var db = new SqlSugarOperation(_connectionString, DbType.Sqlite);

            // 先查询一条记录
            var log = await db.GetById<UploadLog>(1);
            if (log != null)
            {
                // 修改数据
                log.Result = true;
                log.ResponseJson = "{\"success\":true,\"message\":\"已更新\"}";

                // 更新记录
                var result = await db.Update(new List<UploadLog> { log });
                Console.WriteLine($"更新了 {result} 条记录");
            }
        }

        /// <summary>
        /// 示例：删除上传日志记录
        /// </summary>
        public async Task DeleteUploadLogExample()
        {
            using var db = new SqlSugarOperation(_connectionString, DbType.Sqlite);

            // 删除满足条件的记录
            var result = await db.DeleteAsync<UploadLog>(l => l.Duration > 1000);
            Console.WriteLine($"删除了 {result} 条耗时超过1000ms的记录");
        }

        /// <summary>
        /// 示例：事务操作
        /// </summary>
        public async Task TransactionExample()
        {
            using var db = new SqlSugarOperation(_connectionString, DbType.Sqlite);

            try
            {
                // 开始事务
                db.BeginTran();

                // 插入多条记录
                var log1 = new UploadLog
                {
                    ApiName = "事务测试1",
                    ApiUrl = "/api/test1",
                    RequestMethod = "POST",
                    RequestTime = DateTime.Now,
                    Duration = 50,
                    RequestJson = "{}",
                    ResponseJson = "{\"success\":true}",
                    Result = true
                };

                var log2 = new UploadLog
                {
                    ApiName = "事务测试2",
                    ApiUrl = "/api/test2",
                    RequestMethod = "GET",
                    RequestTime = DateTime.Now,
                    Duration = 30,
                    RequestJson = "",
                    ResponseJson = "{\"data\":[1,2,3]}",
                    Result = true
                };

                await db.InsertSync(log1);
                await db.InsertSync(log2);

                // 提交事务
                db.CommitTran();
                Console.WriteLine("事务提交成功");
            }
            catch (Exception ex)
            {
                // 回滚事务
                db.RollbackTran();
                Console.WriteLine($"事务失败: {ex.Message}");
            }
        }
    }
}