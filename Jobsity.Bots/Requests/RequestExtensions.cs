using Microsoft.AspNetCore.Http;

namespace Jobsity.Bots.Requests
{
    public static class RequestExtensions
    {
        public static string GetHeader(this HttpRequest request, string name)
        {
            if (request.Headers.TryGetValue(name, out var value))
            {
                return value;
            }

            return string.Empty;
        }
    }
}
