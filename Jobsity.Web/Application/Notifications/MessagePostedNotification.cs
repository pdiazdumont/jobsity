using System;
using Jobsity.Web.Application.Messages;
using Jobsity.Web.Application.Users;

namespace Jobsity.Web.Application.Notifications
{
	public class MessagePostedNotification : Notification
	{
		public string Text { get; set; }
		public User User { get; set; }

		public MessagePostedNotification(Message message)
		{
			Type = NotificationType.MessagePosted;
			Text = message.Text;
			User = message.User;
		}
	}
}
