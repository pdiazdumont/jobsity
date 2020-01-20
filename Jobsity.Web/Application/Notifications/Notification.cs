using System;

namespace Jobsity.Web.Application.Notifications
{
	public abstract class Notification
	{
		public Guid NotificationId { get; set; } = Guid.NewGuid();
		public NotificationType Type { get; set; }
		public DateTimeOffset NotificationTimestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
