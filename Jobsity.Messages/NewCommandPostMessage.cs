using System;

namespace Jobsity.Messages
{
	public class NewCommandPostMessage
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string CommandName { get; set; }
		public string CommandArguments { get; set; }
	}
}
