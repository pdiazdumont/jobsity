using System;
using Jobsity.Bots.Middlewares.RequestValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Bots.Middlewares
{
    public static class BotMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestValidation(this IApplicationBuilder builder) =>
            builder.UseMiddleware<RequestValidationMiddleware>();

        public static IServiceCollection AddRequestValidation(this IServiceCollection services, IConfiguration configuration)
        {
            var botSecret = configuration["Secret"];

            if (string.IsNullOrEmpty(botSecret))
            {
                throw new ApplicationException("Bot secret must have a value");
            }

            return services.AddSingleton(_ => new RequestValidator(botSecret));
        }
    }
}
