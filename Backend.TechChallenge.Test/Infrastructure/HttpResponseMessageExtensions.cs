using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backend.TechChallenge.Test.Infrastructure;

public static class HttpResponseMessageExtensions
{
    public static async Task<T> ReadContent<T>(this HttpResponseMessage message)
    {
        var result = await message.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(result);
    }
}
