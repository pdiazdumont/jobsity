using System;

namespace Jobsity.Web.Application.Notifications
{
	public class NewUserPostNotification : Notification
	{
		public string Text { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTimeOffset Timestamp { get; set; }

		public NewUserPostNotification()
		{
			Type = NotificationType.NewUserPost;
		}
	}
}
