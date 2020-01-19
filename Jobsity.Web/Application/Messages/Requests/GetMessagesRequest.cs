using System.ComponentModel.DataAnnotations;

namespace Jobsity.Web.Application.Messages.Requests
{
	public class GetMessagesRequest
	{
		public int Limit { get; set; } = 50;
	}
}
