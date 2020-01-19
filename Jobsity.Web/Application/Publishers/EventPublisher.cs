using System.Threading.Tasks;
using Jobsity.Events.Messages;
using Rebus.Bus;

namespace Jobsity.Web.Application.Publishers
{
	public class EventPublisher : IPublisher
    {
		private readonly IBus _bus;

		public EventPublisher(IBus bus)
		{
			_bus = bus;
		}

		public async Task PublishAsync(MessagePostedEvent @event)
        {
			await _bus.Send(@event);
        }
	}
}
