using System.Text.Json;

namespace LTWeb_DinhNgocNang_2280602045.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key,
        object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T GetObjectFromJson<T>(this ISession session, string
        key)
        {
            var value = session.GetString(key);
            return value == null ? default :
            JsonSerializer.Deserialize<T>(value);
        }
    }
}