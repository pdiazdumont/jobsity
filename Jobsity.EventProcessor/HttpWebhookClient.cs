using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Jobsity.Events;
using Newtonsoft.Json;

namespace Jobsity.EventProcessor
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

		public Task<HttpResponseMessage> Post(string url, string secret, Event @event)
		{
			var request = CreateRequest(url, secret, @event);
			return _httpClient.SendAsync(request);
		}

		public HttpRequestMessage CreateRequest(string url, string secret, Event @event)
		{
			var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			var body = JsonConvert.SerializeObject(@event);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(url),
				Content = new StringContent(body)
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
