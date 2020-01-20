using System.ComponentModel.DataAnnotations;

namespace Jobsity.Web.Application.Posts.Requests
{
	public class PostMessageRequest
	{
		[Required]
		public string Text { get; set; }
	}
}
