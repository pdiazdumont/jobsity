using System.Threading.Tasks;
using Jobsity.Web.Application.Notifications;

namespace Jobsity.Web.Application.Publishers
{
	public class EventPublisher : IPublisher
    {
        public Task PublishAsync(Notification notification)
        {
			return Task.CompletedTask;
        }
    }
}
