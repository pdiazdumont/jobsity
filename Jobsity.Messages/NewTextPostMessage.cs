using System;

namespace Jobsity.Messages
{
	public class NewTextPostMessage
    {
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Text { get; set; }
		public Guid UserId { get; set; }
		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
