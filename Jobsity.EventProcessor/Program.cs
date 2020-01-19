using System;
using System.Threading.Tasks;
using Jobsity.Events.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Serilog;

namespace Jobsity.EventProcessor
{
	class Program
    {
        static async Task Main(string[] args)
        {
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.ColoredConsole()
				.CreateLogger();

			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, configuration) =>
				{
					configuration
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddEnvironmentVariables();
				})
				.ConfigureServices((hostingContext, services) =>
				{
					services.AddLogging();
					services.AutoRegisterHandlersFromAssemblyOf<Program>();
					services.AddRebus(configure =>
					{
						return configure
								.Logging(l => l.ColoredConsole())
								.Transport(t => t.UseSqlServer(hostingContext.Configuration["ConnectionStrings:DefaultConnection"], "events"))
								.Routing(r => r.TypeBased().Map<MessagePostedEvent>("events"));
					});
					services.AddHttpClient<IWebhookClient, HttpWebhookClient>();
				})
				.Build();

			try
			{
				builder.Services.UseRebus();

				Log.Information("Starting {0}", "Jobsity.EventProcessor");

				await builder.RunAsync();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Unhandled exception");
				Log.CloseAndFlush();
			}
		}
    }
}
