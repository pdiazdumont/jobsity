using System.Threading.Tasks;
using Jobsity.Events;
using Microsoft.AspNetCore.SignalR;

namespace Jobsity.Web.Application.Publishers
{
	public class WebPublisher : IPublisher
    {
        private readonly IHubContext<ChatHub> _hub;

        public WebPublisher(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task PublishAsync(Event @event)
        {
			await _hub.Clients.All.SendAsync(@event.Type.ToString(), @event);
        }
    }
}
