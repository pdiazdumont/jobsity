﻿using System;

namespace Jobsity.Web.Application.Messages.Responses
{
	public class GetMessageResponse
	{
		public string Text { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTimeOffset Timestamp { get; set; }
	}
}
