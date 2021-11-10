using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Northwind.Store.UI.Web.Internet.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value) where T : class
        {

            string json = JsonSerializer.Serialize(value);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);

            session?.Set(key, serializedResult);
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            T result = default(T);

            byte[] requestEntriesBytes = session.Get(key);

            if (requestEntriesBytes != null && requestEntriesBytes.Length > 0)
            {
                string json = System.Text.Encoding.UTF8.GetString(requestEntriesBytes);
                result = JsonSerializer.Deserialize<T>(json);
            }

            return result;
        }
    }
}
