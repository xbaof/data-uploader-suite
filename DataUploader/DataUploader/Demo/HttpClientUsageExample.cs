using DataUploader.Application;
using DataUploader.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataUploader.Demo
{
    public class HttpClientUsageExample
    {
        /// <summary>
        /// 示例：基本HTTP请求
        /// </summary>
        public async Task BasicRequestExample()
        {
            using var httpClient = new HttpClientWrapper();

            // 设置基地址
            httpClient.SetBaseAddress("https://jsonplaceholder.typicode.com");

            // 设置请求头
            httpClient.SetDefaultHeader("User-Agent", "DataUploader/1.0");
            httpClient.SetContentType("application/json");

            try
            {
                // GET请求
                var posts = await httpClient.GetAsync("/posts");
                Console.WriteLine($"GET响应: {posts.Substring(0, Math.Min(100, posts.Length))}...");

                // GET请求并解析为对象
                var post = await httpClient.GetAsync<Post>("/posts/1");
                Console.WriteLine($"标题: {post.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 示例：带认证的HTTP请求
        /// </summary>
        public async Task AuthenticatedRequestExample()
        {
            using var httpClient = new HttpClientWrapper();

            // 设置基地址
            httpClient.SetBaseAddress("https://api.example.com");

            // 设置认证头

            // 设置其他请求头
            httpClient.SetDefaultHeader("Accept", "application/json");

            try
            {
                // GET请求
                var userData = await httpClient.GetAsync<User>("/user/profile");
                Console.WriteLine($"用户名: {userData.Name}");

                // POST请求
                var newPost = new Post
                {
                    Title = "新文章",
                    Content = "文章内容",
                    UserId = 1
                };

                var createdPost = await httpClient.PostAsync<Post>("/posts", newPost);
                Console.WriteLine($"创建的文章ID: {createdPost.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 示例：上传日志到远程API
        /// </summary>
        public async Task UploadLogExample()
        {
            using var httpClient = new HttpClientWrapper();

            // 设置基地址（从配置中获取）
            httpClient.SetBaseAddress("https://api.yourcompany.com");


            // 设置请求头
            httpClient.SetDefaultHeader("X-API-Version", "1.0");

            try
            {
                // 创建上传日志对象
                var uploadLog = new UploadLog
                {
                    ApiName = "用户数据同步",
                    ApiUrl = "/api/sync/users",
                    RequestMethod = "POST",
                    RequestTime = DateTime.Now,
                    Duration = 1250,
                    RequestJson = "{\"batchSize\":100,\"syncType\":\"full\"}",
                    ResponseJson = "{\"success\":true,\"processed\":100,\"errors\":0}",
                    Result = true
                };

                // 发送POST请求
                var response = await httpClient.PostAsync<ApiResponse>("/api/logs/upload", uploadLog);
                Console.WriteLine($"日志上传结果: {response.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"日志上传失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 示例：批量处理请求
        /// </summary>
        public async Task BatchRequestExample()
        {
            using var httpClient = new HttpClientWrapper();

            // 设置基地址
            httpClient.SetBaseAddress("https://api.example.com");

            // 设置请求头
            httpClient.SetDefaultHeaders(new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "User-Agent", "DataUploader/1.0" },
                { "X-Requested-With", "XMLHttpRequest" }
            });

            try
            {
                // 批量发送多个请求
                var tasks = new List<Task>();

                for (int i = 1; i <= 5; i++)
                {
                    var postId = i;
                    var task = Task.Run(async () =>
                    {
                        var post = await httpClient.GetAsync<Post>($"/posts/{postId}");
                        Console.WriteLine($"获取文章 {post.Id}: {post.Title}");
                    });

                    tasks.Add(task);
                }

                // 等待所有请求完成
                await Task.WhenAll(tasks);
                Console.WriteLine("所有请求已完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"批量请求失败: {ex.Message}");
            }
        }
    }

    // 示例数据模型
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}