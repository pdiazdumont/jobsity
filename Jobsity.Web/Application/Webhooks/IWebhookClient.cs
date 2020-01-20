using System.Net.Http;
using System.Threading.Tasks;
using Jobsity.Events;

namespace Jobsity.Web.Application.Webhooks
{
	public interface IWebhookClient
	{
		Task<HttpResponseMessage> Post(string url, string secret, Event payload);
	}
}
