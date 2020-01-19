using System;

namespace Jobsity.Web.Application.Notifications
{
	public abstract class Notification
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public NotificationType Type { get; set; }
		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
