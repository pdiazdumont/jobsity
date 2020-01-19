using System.Threading.Tasks;
using Jobsity.Events.Messages;
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

        public async Task PublishAsync(MessagePostedEvent @event)
        {
			await _hub.Clients.All.SendAsync(@event.Type.ToString(), @event);
        }
    }
}
