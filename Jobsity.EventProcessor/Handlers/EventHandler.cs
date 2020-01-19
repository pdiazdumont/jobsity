using System.Threading.Tasks;
using Jobsity.Events;
using Rebus.Handlers;

namespace Jobsity.EventProcessor.Handlers
{
	public class EventHandler : IHandleMessages<Event>
	{
		public Task Handle(Event message)
		{
			return Task.CompletedTask;
		}
	}
}
