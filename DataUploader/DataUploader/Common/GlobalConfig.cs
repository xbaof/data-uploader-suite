using Newtonsoft.Json.Linq;

namespace DataUploader
{
    public class GlobalConfig
    {
        public const string UniqueAppId = "02141185-AB77-4517-8F4D-7D7AB10E3A04";
        public const string UploaderDB = "DataSource=./DataBase/DataUploader.db";
        public static string ThirdDB { get; set; }
        public static string BaseUrl { get; set; }
        public static string PKI { get; set; }

        public static List<TasksQz> Tasks { get; set; }

        public static void LoadFromJson(string json)
        {
            var obj = JObject.Parse(json ?? "{}");
            ThirdDB = obj.Value<string>("ThirdDB");
            BaseUrl = obj.Value<string>("BaseUrl");
            PKI = obj.Value<string>("PKI");
            Tasks = obj["Tasks"]?.ToObject<List<TasksQz>>() ?? [];
        }
    }
}
