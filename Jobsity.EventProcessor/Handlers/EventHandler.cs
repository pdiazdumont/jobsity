using System.Threading.Tasks;
using Jobsity.Events;
using Rebus.Handlers;
using Serilog;

namespace Jobsity.EventProcessor.Handlers
{
	public class EventHandler : IHandleMessages<Event>
	{
		private readonly IWebhookClient _webhookClient;
		private readonly ILogger _logger = Log.Logger.ForContext<EventHandler>();

		public EventHandler(IWebhookClient webhookClient)
		{
			_webhookClient = webhookClient;
		}

		public async Task Handle(Event @event)
		{
			var logger = _logger.ForContext("Event", @event, destructureObjects: true);

			logger.Information("Event {0} with type {1} received", @event.EventId, @event.Type);

			var result = await _webhookClient.Post("https://httpstat.us/", "", @event);

			if (!result.IsSuccessStatusCode)
			{
				logger.Information("Event {0} with type {1} failed posting", @event.EventId, @event.Type);
			}
		}
	}
}
