using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using Jobsity.Bots.Stock.Api.Model;
using Microsoft.AspNetCore.WebUtilities;

namespace Jobsity.Bots.Stock.Api.Services
{
	public class HttpStooqClient : IStooqClient
    {
        private readonly HttpClient _httpClient;

        public HttpStooqClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetStockValue(string name)
        {
            var parameters = new Dictionary<string, string>
            {
                { "s", name },
                { "f", "sd2t2ohlcv" },
                { "h", string.Empty },
                { "e", "csv" }
            };

            var request = CreateGetRequest(parameters);
            var response = await SendRequest(request);

            return response.Close;
        }

        private HttpRequestMessage CreateGetRequest(IDictionary<string, string> queryParameters = null)
        {
            var endpoint = QueryHelpers.AddQueryString(string.Empty, queryParameters);
            return new HttpRequestMessage(HttpMethod.Get, endpoint);
        }

        private async Task<StockInformation> SendRequest(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            using (var csv = new CsvReader(new StringReader(result), CultureInfo.InvariantCulture))
            {
				var records = csv.GetRecords<StockInformation>();
				return records.FirstOrDefault();
			}
        }
    }
}
