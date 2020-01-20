using System;

namespace Jobsity.Web.Application.Notifications
{
	public class NewBotPostNotification : Notification
	{
		public string Text { get; set; }
		public DateTimeOffset Timestamp { get; set; }

		public NewBotPostNotification()
		{
			Type = NotificationType.NewBotPost;
		}
	}
}
