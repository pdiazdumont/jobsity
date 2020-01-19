using System;

namespace Jobsity.Events.Messages
{
	public class MessagePostedEvent : Event
	{
		public string Text { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTimeOffset Timestamp { get; set; }

		public MessagePostedEvent()
		{
			Type = EventType.MessagePosted;
		}
	}
}
