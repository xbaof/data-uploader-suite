using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System.Text;


namespace DataUploader.Application
{
    public class HttpClientWrapper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #region 设置请求头

        /// <summary>
        /// 设置默认请求头
        /// </summary>
        /// <param name="key">请求头键</param>
        /// <param name="value">请求头值</param>
        public void SetDefaultHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("请求头键不能为空", nameof(key));

            // 如果已存在则先移除
            if (_httpClient.DefaultRequestHeaders.Contains(key))
                _httpClient.DefaultRequestHeaders.Remove(key);

            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        /// <summary>
        /// 批量设置默认请求头
        /// </summary>
        /// <param name="headers">请求头字典</param>
        public void SetDefaultHeaders(Dictionary<string, string> headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            foreach (var header in headers)
            {
                SetDefaultHeader(header.Key, header.Value);
            }
        }

        /// <summary>
        /// 设置Content-Type
        /// </summary>
        /// <param name="contentType">Content-Type值</param>
        public void SetContentType(string contentType)
        {
            SetDefaultHeader("Content-Type", contentType);
        }

        #endregion

        #region GET请求

        /// <summary>
        /// 发送GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>响应内容</returns>
        public async Task<string> GetAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL不能为空", nameof(url));

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送GET请求并解析为指定类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns>解析后的对象</returns>
        public async Task<T> GetAsync<T>(string url)
        {
            var json = await GetAsync(url);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region POST请求

        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求内容</param>
        /// <returns>响应内容</returns>
        public async Task<string> PostAsync(string url, string content)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL不能为空", nameof(url));

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送POST请求（JSON）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <returns>响应内容</returns>
        public async Task<string> PostAsync(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return await PostAsync(url, json);
        }

        /// <summary>
        /// 发送POST请求并解析为指定类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求内容</param>
        /// <returns>解析后的对象</returns>
        public async Task<T> PostAsync<T>(string url, string content)
        {
            var json = await PostAsync(url, content);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送POST请求（JSON）并解析为指定类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <returns>解析后的对象</returns>
        public async Task<T> PostAsync<T>(string url, object data)
        {
            var json = await PostAsync(url, data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region PUT请求

        /// <summary>
        /// 发送PUT请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求内容</param>
        /// <returns>响应内容</returns>
        public async Task<string> PutAsync(string url, string content)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL不能为空", nameof(url));

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, stringContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送PUT请求（JSON）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <returns>响应内容</returns>
        public async Task<string> PutAsync(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return await PutAsync(url, json);
        }

        /// <summary>
        /// 发送PUT请求并解析为指定类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求内容</param>
        /// <returns>解析后的对象</returns>
        public async Task<T> PutAsync<T>(string url, string content)
        {
            var json = await PutAsync(url, content);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 发送PUT请求（JSON）并解析为指定类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <returns>解析后的对象</returns>
        public async Task<T> PutAsync<T>(string url, object data)
        {
            var json = await PutAsync(url, data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region DELETE请求

        /// <summary>
        /// 发送DELETE请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>响应内容</returns>
        public async Task<string> DeleteAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL不能为空", nameof(url));

            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送DELETE请求并解析为指定类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns>解析后的对象</returns>
        public async Task<T> DeleteAsync<T>(string url)
        {
            var json = await DeleteAsync(url);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region 其他设置

        /// <summary>
        /// 设置请求超时时间
        /// </summary>
        /// <param name="timeout">超时时间</param>
        public void SetTimeout(TimeSpan timeout)
        {
            _httpClient.Timeout = timeout;
        }

        /// <summary>
        /// 设置基地址
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        public void SetBaseAddress(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("基地址不能为空", nameof(baseUrl));

            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        #endregion

        #region IDisposable实现

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}