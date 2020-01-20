using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jobsity.Messages;
using Jobsity.Web.Application.Posts.Requests;
using Jobsity.Web.Application.Posts.Responses;
using Jobsity.Web.Application.Users;
using Jobsity.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rebus.Bus;

namespace Jobsity.Web.Application.Posts
{
	[ApiController]
	[Route("posts")]
	public class PostsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<User> _userManager;
		public readonly IBus _bus;

		public PostsController(ApplicationDbContext dbContext, UserManager<User> userManager, IBus bus)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_bus = bus;
		}

		[HttpGet]
		public async Task<ActionResult> GetMessages([FromQuery] GetPostsRequest request)
		{
			var messages = await _dbContext.Posts
										.Include(message => message.User)
										.OrderByDescending(message => message.Timestamp)
										.Take(request.Limit)
										.ToListAsync();

			var response = messages.Select(message => new PostResponse
			{
				Text = message.Text,
				Timestamp = message.Timestamp,
				UserId = message.User.Id,
				UserName = message.User.UserName
			});

			return Ok(response);
		}

		[HttpPost]
		[Route("new")]
		public async Task<ActionResult> NewPost([FromBody] PostMessageRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var user = await _userManager.GetUserAsync(User);

			if (user != null && request.Text.StartsWith("/"))
			{
				await HandleCommandPost(request.Text);
			}
			else
			{
				await HandleTextPost(request.Text, user);
			}

			return Ok();
		}

		private Task HandleTextPost(string text, User user) =>
			_bus.Send(new NewTextPostMessage
			{
				Text = text,
				UserId = user == null ? Guid.Empty : Guid.Parse(user.Id)
			});

		private Task HandleCommandPost(string text)
		{
			var commandRegex = new Regex(@"\/([^=& ]*)=([^=& ]+)");
			var matches = commandRegex.Matches(text).FirstOrDefault();

			if (matches == null)
			{
				return Task.CompletedTask;
			}

			return _bus.Send(new NewCommandPostMessage
			{
				CommandName = matches.Groups[1].ToString(),
				CommandArguments = matches.Groups[2].ToString()
			});
		}
	}
}
