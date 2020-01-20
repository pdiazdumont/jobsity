using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jobsity.Bots.Stock.Api.Services
{
	public class HttpJobsityClient : IJobsityClient
	{
		private readonly HttpClient _httpClient;

		public HttpJobsityClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task NewPost(string text)
		{
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri($"{_httpClient.BaseAddress}posts/new"),
				Method = HttpMethod.Post,
				Content = new StringContent(JsonConvert.SerializeObject(new
				{
					Text = text
				}), Encoding.UTF8, MediaTypeNames.Application.Json)
			};

			var result = await _httpClient.SendAsync(request);
		}
	}
}
