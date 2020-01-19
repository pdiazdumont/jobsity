using System;
using System.ComponentModel.DataAnnotations;

namespace Jobsity.Web.Application.Messages.Requests
{
	public class PostMessageRequest
	{
		[Required]
		public string Text { get; set; }
	}
}
