using System;
using System.ComponentModel.DataAnnotations;

namespace Jobsity.Web.Application.Messages.Requests
{
	public class GetMessagesRequest
	{
		[Required]
		public int Limit { get; set; } = 50;
		public DateTimeOffset Timestamp { get; set; }
	}
}
