using System.Collections.Generic;
using System.Text.Json.Serialization;
using Jobsity.Web.Application.Messages;
using Microsoft.AspNetCore.Identity;

namespace Jobsity.Web.Application.Users
{
	public class User : IdentityUser
	{
		[JsonIgnore]
		public List<Message> Messages { get; set; } = new List<Message>();
	}
}
