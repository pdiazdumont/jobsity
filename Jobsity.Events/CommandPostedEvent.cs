namespace Jobsity.Events
{
	public class CommandPostedEvent : Event
	{
		public string CommandName { get; set; }
		public string CommandArguments { get; set; }

		public CommandPostedEvent()
		{
			Type = EventType.CommandPosted;
		}
	}
}
