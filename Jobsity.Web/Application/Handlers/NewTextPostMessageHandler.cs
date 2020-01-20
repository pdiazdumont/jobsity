using System.Threading.Tasks;
using Jobsity.Messages;
using Jobsity.Web.Application.Notifications;
using Jobsity.Web.Application.Posts;
using Jobsity.Web.Application.Users;
using Jobsity.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Rebus.Handlers;
using Serilog;

namespace Jobsity.Web.Application.Handlers
{
	public class NewTextPostMessageHandler : IHandleMessages<NewTextPostMessage>
	{
		private readonly UserManager<User> _userManager;
		private readonly ApplicationDbContext _dbContext;
		private readonly IHubContext<ChatHub> _hub;
		private readonly ILogger _logger = Log.Logger.ForContext<NewTextPostMessageHandler>();

		public NewTextPostMessageHandler(UserManager<User> userManager, ApplicationDbContext dbContext, IHubContext<ChatHub> hub)
		{
			_hub = hub;
			_dbContext = dbContext;
			_userManager = userManager;
		}

		public async Task Handle(NewTextPostMessage message)
		{
			var logger = _logger.ForContext("Message", message, destructureObjects: true);

			logger.Information("Message {0} with type {1} received", message.Id, typeof(NewTextPostMessage));

			var user = await _userManager.FindByIdAsync(message.UserId.ToString());

			var notificationType = user == null ? NotificationType.NewBotPost : NotificationType.NewUserPost;

			if (notificationType == NotificationType.NewUserPost)
			{
				user.Posts.Add(new Post
				{
					Text = message.Text
				});

				await _dbContext.SaveChangesAsync();

				await _hub.Clients.All.SendAsync(notificationType.ToString(), new NewUserPostNotification
				{
					Text = message.Text,
					UserId = user.Id,
					UserName = user.UserName,
					Timestamp = message.Timestamp
				});
			}
			else
			{
				await _hub.Clients.All.SendAsync(notificationType.ToString(), new NewBotPostNotification
				{
					Text = message.Text,
					Timestamp = message.Timestamp
				});
			}
		}
	}
}
