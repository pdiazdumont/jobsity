using System;
using System.Threading.Tasks;
using Jobsity.EventProcessor.Data;
using Jobsity.Events;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Serilog;

namespace Jobsity.EventProcessor.Handlers
{
	public class EventHandler : IHandleMessages<Event>
	{
		private readonly IWebhookClient _webhookClient;
		private readonly ApplicationDbContext _dbContext;
		private readonly ILogger _logger = Log.Logger.ForContext<EventHandler>();

		public EventHandler(IWebhookClient webhookClient, ApplicationDbContext dbContext)
		{
			_webhookClient = webhookClient;
			_dbContext = dbContext;
		}

		public async Task Handle(Event @event)
		{
			var logger = _logger.ForContext("Event", @event, destructureObjects: true);

			logger.Information("Event {0} with type {1} received", @event.EventId, @event.Type);

			var bots = await _dbContext.Bots.ToListAsync();

			foreach (var bot in bots)
			{
				try
				{
					var result = await _webhookClient.Post(bot.Url, bot.Secret, @event);

					if (!result.IsSuccessStatusCode)
					{
						logger.Information("Event {0} with type {1} failed posting for {2}", @event.EventId, @event.Type, bot.Name);
					}
				}
				catch (Exception ex)
				{
					logger.Warning(ex, "Event {0} with type {1} failed posting for {2}", @event.EventId, @event.Type, bot.Name);
				}
			}
		}
	}
}
