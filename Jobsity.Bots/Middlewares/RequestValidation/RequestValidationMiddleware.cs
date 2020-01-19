using System;
using System.Threading.Tasks;
using Jobsity.Bots.Middlewares.RequestValidation;
using Microsoft.AspNetCore.Http;

namespace Jobsity.Bots.Middlewares
{
    public class RequestValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestValidator _validator;

        public RequestValidationMiddleware(RequestDelegate next, RequestValidator validator)
        {
            _next = next;
            _validator = validator;
        }

        public async Task Invoke(HttpContext httpContext)
        {
			if (httpContext.Request.Path.StartsWithSegments("/events"))
			{
				var isValid = await _validator.Validate(httpContext.Request);

				if (!isValid)
				{
					throw new ApplicationException("Invalid request!");
				}
			}

			await _next(httpContext);
        }
    }
}
