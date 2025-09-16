using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FinanceTracker.Utils.Extensions;

public static class UtilityExtenstions
{
    public static string Serialize<T>(this T @object, JsonSerializerSettings? settings = null) {
        settings ??= new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        return JsonConvert.SerializeObject(@object, settings);
    }
    
    public static ApiResponse<T> ToApiResponse<T>(this T data, string message, int code)
        => new ApiResponse<T>(message, code, data);
}
