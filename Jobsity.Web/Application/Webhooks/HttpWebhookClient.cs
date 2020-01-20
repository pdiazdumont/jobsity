using System;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Jobsity.Events;
using Newtonsoft.Json;

namespace Jobsity.Web.Application.Webhooks
{
	public class HttpWebhookClient : IWebhookClient
	{
		private readonly HttpClient _httpClient;
		private const string _requestTimestampHeaderName = "X-Jobsity-Request-Timestamp";
		private const string _signatureHeaderName = "X-Jobsity-Signature";

		public HttpWebhookClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public Task<HttpResponseMessage> Post(string url, string secret, Event payload)
		{
			var request = CreateRequest(url, secret, payload);
			return _httpClient.SendAsync(request);
		}

		public HttpRequestMessage CreateRequest(string url, string secret, Event payload)
		{
			var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			var body = JsonConvert.SerializeObject(payload);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(url),
				Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
			};

			request.Headers.Add(_requestTimestampHeaderName, timestamp.ToString());
			request.Headers.Add(_signatureHeaderName, CalculateSignature(timestamp, body, secret));

			return request;
		}

		private string CalculateSignature(long timestamp, string body, string secret)
		{
			var signatureString = Encoding.UTF8.GetBytes($"{timestamp}:{body}");

			using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
			{
				var hash = hmac.ComputeHash(signatureString);
				return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
			}
		}
	}
}
