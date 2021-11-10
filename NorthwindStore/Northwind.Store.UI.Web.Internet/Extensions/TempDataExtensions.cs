using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Northwind.Store.UI.Web.Internet.Extensions
{
    public static class TempDataExtensions
    {
        public static void SetAsJson<T>(this ITempDataDictionary tempData, string key, T data)
        {
            tempData[key] = JsonSerializer.Serialize(data);
        }

        public static T GetFromJson<T>(this ITempDataDictionary tempData, string key)
        {
            if (tempData.ContainsKey(key))
            {
                var v = tempData[key];

                if (v is T)
                {
                    return (T)v;
                }

                if (v is string && typeof(T) != typeof(string))
                {
                    return JsonSerializer.Deserialize<T>((string)v);
                }
            }
            return default(T);
        }
    }
}
