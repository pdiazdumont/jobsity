using System.IO;
using System.Text;
using System.Threading.Tasks;
using Jobsity.Bots.Middlewares.RequestValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace Jobsity.Bots.Stock.Tests
{
	public class RequestValidatorTests
    {
		private readonly RequestValidator _validator = new RequestValidator("00000");
		private readonly object _requestPayload = new
		{
			Foo = "Bar"
		};

        [Fact]
        public async Task When_request_is_valid()
        {
			var request = CreateRequest("123456", "683bad41f2a4a15d143b108d0bf2387dc69932d00eea44900ce1efe0350737e7");

			Assert.True(await _validator.Validate(request));
		}

		[Fact]
		public async Task When_request_is_invalid()
		{
			var request = CreateRequest("123456", "wrong");

			Assert.False(await _validator.Validate(request));
		}

		private HttpRequest CreateRequest(string timestamp, string signature)
		{
			var context = new DefaultHttpContext();
			context.Request.Headers.Add("X-Jobsity-Request-Timestamp", timestamp);
			context.Request.Headers.Add("X-Jobsity-Signature", signature);
			context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_requestPayload)));

			return context.Request;
		}
	}
}
