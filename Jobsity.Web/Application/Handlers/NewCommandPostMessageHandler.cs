using System;
using System.Threading.Tasks;
using Jobsity.Events;
using Jobsity.Messages;
using Jobsity.Web.Application.Webhooks;
using Jobsity.Web.Data;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Serilog;

namespace Jobsity.Web.Application.Handlers
{
	public class NewCommandPostMessageHandler : IHandleMessages<NewCommandPostMessage>
	{
		private readonly IWebhookClient _webhookClient;
		private readonly ApplicationDbContext _dbContext;
		private readonly ILogger _logger = Log.Logger.ForContext<NewCommandPostMessageHandler>();

		public NewCommandPostMessageHandler(IWebhookClient webhookClient, ApplicationDbContext dbContext)
		{
			_webhookClient = webhookClient;
			_dbContext = dbContext;
		}

		public async Task Handle(NewCommandPostMessage message)
		{
			var logger = _logger.ForContext("Message", message, destructureObjects: true);

			logger.Information("Message {0} with type {1} received", message.Id, typeof(NewCommandPostMessage));

			var bots = await _dbContext.Bots.ToListAsync();

			foreach (var bot in bots)
			{
				try
				{
					var payload = new CommandPostedEvent
					{
						CommandName = message.CommandName,
						CommandArguments = message.CommandArguments
					};

					var result = await _webhookClient.Post(bot.Url, bot.Secret, payload);

					if (!result.IsSuccessStatusCode)
					{
						logger.Information("Message {0} with type {1} failed posting for {2}", message.Id, typeof(NewCommandPostMessage), bot.Name);
					}
				}
				catch (Exception ex)
				{
					logger.Warning(ex, "Message {0} with type {1} failed posting for {2}", message.Id, typeof(NewCommandPostMessage), bot.Name);
				}
			}
		}
	}
}
