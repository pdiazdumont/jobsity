using System.Threading.Tasks;
using Jobsity.Web.Application.Notifications;
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

        public async Task PublishAsync(Notification notification)
        {
			await _hub.Clients.All.SendAsync(notification.Type.ToString(), notification);
        }
    }
}
