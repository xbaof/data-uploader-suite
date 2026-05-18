using Quartz.Impl;
using Quartz;
using System.Reflection;
using System.Collections.Specialized;

namespace DataUploader.Tasks
{
    /// <summary>
    /// 任务调度器注册类
    /// </summary>
    public class SchedulerRegister
    {
        public static readonly Serilog.ILogger _log = Serilog.Log.ForContext(typeof(SchedulerRegister));
        private static IScheduler _scheduler;

        /// <summary>
        /// 初始化调度器
        /// </summary>
        public static async Task InitScheduler()
        {
            try
            {
                // 配置调度器工厂
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

                var factory = new StdSchedulerFactory(props);
                _scheduler = await factory.GetScheduler();

                // 检查是否有任务需要注册
                if (GlobalConfig.Tasks == null || GlobalConfig.Tasks.Count == 0)
                {
                    _log.Information("没有配置定时任务");
                    return;
                }
                int index = 0;
                foreach (var task in GlobalConfig.Tasks)
                {
                    index++;
                    // 跳过未启用的任务
                    if (!task.Enable)
                    {
                        _log.Information($"任务 [{task.Name}] 未启用，跳过注册");
                        continue;
                    }

                    // 验证任务配置
                    if (string.IsNullOrEmpty(task.Name) ||
                        string.IsNullOrEmpty(task.Cron) ||
                        string.IsNullOrEmpty(task.AssemblyName) ||
                        string.IsNullOrEmpty(task.ClassName))
                    {
                        _log.Warning($"任务 [{task.Name}] 配置不完整，跳过注册");
                        continue;
                    }

                    try
                    {
                        if (!IsValidCronExpression(task.Cron))
                        {
                            _log.Warning($"任务 [{task.Name}] 的Cron表达式无效: {task.Cron}，跳过注册");
                            continue;
                        }

                        // 加载程序集
                        Assembly assembly = Assembly.Load(new AssemblyName(task.AssemblyName));
                        if (assembly == null)
                        {
                            _log.Warning($"无法加载程序集 [{task.AssemblyName}]，跳过任务[{task.Name}]的注册");
                            continue;
                        }

                        // 获取任务类型
                        Type jobType = assembly.GetType($"{task.AssemblyName}.{task.ClassName}");
                        if (jobType == null)
                        {
                            _log.Warning($"在程序集 [{task.AssemblyName}] 中找不到类型 [{task.ClassName}]，跳过任务 {task.Name} 的注册");
                            continue;
                        }

                        // 验证类型是否实现了IJob接口
                        if (!typeof(IJob).IsAssignableFrom(jobType))
                        {
                            _log.Warning($"类型[{task.ClassName}]没有实现IJob接口，跳过任务 [{task.Name}] 的注册");
                            continue;
                        }

                        // 创建任务详情
                        IJobDetail job = JobBuilder.Create(jobType)
                            .WithIdentity($"Identity{index}", "group1") // 添加分组
                            .WithDescription(task.Name) // 使用任务名称作为描述
                            .Build();

                        job.JobDataMap.Add("JobParam", task.Params ?? "");

                        // 创建触发器
                        var trigger = TriggerBuilder.Create()
                            .WithIdentity($"Identity{index}-trigger", "group1") // 添加分组
                            .WithCronSchedule(task.Cron)
                            .ForJob(job.Key)
                            .Build();

                        // 调度任务
                        await _scheduler.ScheduleJob(job, trigger);
                        _log.Information($"任务 [{task.Name}] 注册成功，Cron表达式: {task.Cron}");
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex, $"注册任务 [{task.Name}] 时发生错误");
                    }
                }

                // 启动调度器
                await _scheduler.Start();
                _log.Information("任务调度器初始化完成");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "任务调度器初始化失败");
            }
        }

        /// <summary>
        /// 停止调度器
        /// </summary>
        public static async Task ShutdownScheduler()
        {
            if (_scheduler != null && !_scheduler.IsShutdown)
            {
                await _scheduler.Shutdown();
                _log.Information("任务调度器已停止");
            }
        }

        /// <summary>
        /// 验证Cron表达式是否有效
        /// </summary>
        /// <param name="cronExpression">Cron表达式</param>
        /// <returns>是否有效</returns>
        private static bool IsValidCronExpression(string cronExpression)
        {
            try
            {
                // 使用Quartz的CronExpression类验证表达式
                new CronExpression(cronExpression);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}