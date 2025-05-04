using System.Text.Json;
using System.Text.Json.Serialization;

namespace ACRViewer.BlazorServer.Core.Utilities
{
    internal static class ResponseExtensions
    {
        internal static async Task<T?> ToResponse<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<T>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return responseObject;
        }
    }
}
