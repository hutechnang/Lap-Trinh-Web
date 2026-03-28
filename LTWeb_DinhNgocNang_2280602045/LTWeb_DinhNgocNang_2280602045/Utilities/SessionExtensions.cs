using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LTWeb_DinhNgocNang_2280602045.Utilities // Điều chỉnh namespace theo cấu trúc dự án
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}