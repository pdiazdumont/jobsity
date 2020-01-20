using System;

namespace Jobsity.Web.Application.Posts.Responses
{
	public class PostResponse
	{
		public string Text { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTimeOffset Timestamp { get; set; }
	}
}
