using System;
using System.Threading.Tasks;
using Jobsity.Events.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace Jobsity.EventProcessor
{
	class Program
    {
        static async Task Main(string[] args)
        {
			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, configuration) =>
				{
					configuration
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddEnvironmentVariables();
				})
				.ConfigureServices((hostingContext, services) =>
				{
					services.AutoRegisterHandlersFromAssemblyOf<Program>();
					services.AddRebus(configure =>
					{
						return configure
								.Logging(l => l.ColoredConsole())
								.Transport(t => t.UseSqlServer(hostingContext.Configuration["ConnectionStrings:DefaultConnection"], "events"))
								.Routing(r => r.TypeBased().Map<MessagePostedEvent>("events"));
					});
				})
				.Build();

			try
			{
				builder.Services.UseRebus();
				await builder.RunAsync();
			}
			catch (Exception ex)
			{
				
			}
		}
    }
}
