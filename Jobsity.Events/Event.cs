using System;

namespace Jobsity.Events
{
    public class Event
    {
		public EventType Type { get; set; }
		public Guid EventId { get; set; }
		public DateTimeOffset EventTimestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
