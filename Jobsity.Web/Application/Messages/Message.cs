using System;
using Jobsity.Web.Application.Users;

namespace Jobsity.Web.Application.Messages
{
	public class Message
	{
		public int Id { get; set; }
		public User User { get; set; }
		public string Text { get; set; }
		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
