using System.Net.Http;
using System.Threading.Tasks;
using Jobsity.Events;

namespace Jobsity.EventProcessor
{
	public interface IWebhookClient
	{
		Task<HttpResponseMessage> Post(string url, string secret, Event @event);
	}
}
