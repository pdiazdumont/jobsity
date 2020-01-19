using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobsity.Events;
using Jobsity.Events.Messages;
using Jobsity.Web.Application.Messages.Requests;
using Jobsity.Web.Application.Messages.Responses;
using Jobsity.Web.Application.Publishers;
using Jobsity.Web.Application.Users;
using Jobsity.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Web.Application.Messages
{
	[ApiController]
	[Authorize]
	[Route("messages")]
	public class MessagesController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<User> _userManager;
		private readonly IEnumerable<IPublisher> _publishers;

		public MessagesController(ApplicationDbContext dbContext, UserManager<User> userManager, IEnumerable<IPublisher> publishers)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_publishers = publishers;
		}

		[HttpGet]
		public async Task<ActionResult> GetMessages([FromQuery] GetMessagesRequest request)
		{
			var messages = await _dbContext.Messages
										.Include(message => message.User)
										.OrderByDescending(message => message.Timestamp)
										.Take(request.Limit)
										.ToListAsync();

			var response = messages.Select(message => new GetMessageResponse
			{
				Text = message.Text,
				Timestamp = message.Timestamp,
				UserId = message.User.Id,
				UserName = message.User.UserName
			});

			return Ok(response);
		}

		[HttpPost]
		[Route("post")]
		public async Task<ActionResult> PostMessage([FromBody] PostMessageRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var message = new Message
			{
				Text = request.Text
			};

			var user = await _userManager.GetUserAsync(User);
			user.Messages.Add(message);

			await _dbContext.SaveChangesAsync();

			var @event = CreateMessagePostedEvent(message);

			foreach (var publisher in _publishers)
			{
				await publisher.PublishAsync(@event);
			}

			return Ok();
		}

		public MessagePostedEvent CreateMessagePostedEvent(Message message) =>
			new MessagePostedEvent
			{
				Text = message.Text,
				UserId = message.User.Id,
				UserName = message.User.UserName,
				Timestamp = message.Timestamp
			};
	}
}
