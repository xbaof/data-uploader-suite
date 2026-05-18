using SqlSugar;
using Npoi.Mapper.Attributes;

namespace DataUploader.Models
{
    /// <summary>
    /// 上传日志实体
    /// </summary>
    [SugarTable("upload_log", "上传日志")]
    public class UploadLog
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        [Ignore]
        public long Id { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [SugarColumn(ColumnName = "api_name")]
        [Column("接口名称")]
        public string ApiName { get; set; } = string.Empty;

        /// <summary>
        /// 接口地址
        /// </summary>
        [SugarColumn(ColumnName = "api_url")]
        [Column("接口地址")]
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// 请求方式
        /// </summary>
        [SugarColumn(ColumnName = "request_method")]
        [Column("请求方式")]
        public string RequestMethod { get; set; } = string.Empty;


        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(ColumnName = "request_time")]
        [Ignore]
        public DateTime RequestTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Column("时间")]
        public string RequestTimeFormat
        {
            get
            {
                return RequestTime.ToString("yyyy-MM-dd HH:mm:dd");
            }
        }

        /// <summary>
        /// 附加说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Column("附加说明")]
        public string Note
        {
            get
            {
                return Result ? "成功" : "失败";
            }
        }
        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        [SugarColumn(ColumnName = "duration")]
        [Column("耗时(毫秒)")]
        public long Duration { get; set; }

        /// <summary>
        /// 请求JSON
        /// </summary>
        [SugarColumn(ColumnName = "request_json", ColumnDataType = "text")]
        [Column("请求")]
        public string RequestJson { get; set; } = string.Empty;

        /// <summary>
        /// 响应JSON
        /// </summary>
        [SugarColumn(ColumnName = "response_json", ColumnDataType = "text")]
        [Column("响应")]
        public string ResponseJson { get; set; } = string.Empty;

        /// <summary>
        /// 请求结果（成功/失败）
        /// </summary>
        [SugarColumn(ColumnName = "result")]
        [Ignore]
        public bool Result { get; set; } = false;


    }
}