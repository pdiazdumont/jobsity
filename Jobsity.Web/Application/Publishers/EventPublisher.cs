using System.Threading.Tasks;
using Jobsity.Events;
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

		public async Task PublishAsync(Event @event)
        {
			await _bus.Send(@event);
        }
    }
}
