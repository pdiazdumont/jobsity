using System.Collections.Generic;
using System.Text.Json.Serialization;
using Jobsity.Web.Application.Posts;
using Microsoft.AspNetCore.Identity;

namespace Jobsity.Web.Application.Users
{
	public class User : IdentityUser
	{
		[JsonIgnore]
		public List<Post> Posts { get; set; } = new List<Post>();
	}
}
