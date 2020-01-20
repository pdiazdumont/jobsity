using System;

namespace Jobsity.Events
{
    public class Event
    {
		public EventType Type { get; set; }
		public Guid EventId { get; set; } = Guid.NewGuid();
		public DateTimeOffset EventTimestamp { get; set; }
	}
}
