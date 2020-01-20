using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Jobsity.Bots.Requests;
using Microsoft.AspNetCore.Http;

namespace Jobsity.Bots.Middlewares.RequestValidation
{
    public class RequestValidator
    {
        private readonly string _botSecret;
        private readonly string _timestampHeaderName = "X-Jobsity-Request-Timestamp";
        private readonly string _signatureHeadername = "X-Jobsity-Signature";

        public RequestValidator(string botSecret)
        {
            _botSecret = botSecret;
        }

        public async Task<bool> Validate(HttpRequest request)
        {
            var timestamp = request.GetHeader(_timestampHeaderName);

            using (var streamReader = new StreamReader(request.Body, Encoding.UTF8))
            {
                var body = await streamReader.ReadToEndAsync();

                var signatureString = Encoding.UTF8.GetBytes($"{timestamp}:{body}");

                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_botSecret)))
                {
                    var hash = hmac.ComputeHash(signatureString);

                    var signature = request.GetHeader(_signatureHeadername);

					request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

					return string.Equals(BitConverter.ToString(hash).Replace("-", string.Empty).ToLower(), signature);
                }
            }
        }
    }
}
