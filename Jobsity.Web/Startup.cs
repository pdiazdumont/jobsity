using Jobsity.Events.Messages;
using Jobsity.Web.Application;
using Jobsity.Web.Application.Publishers;
using Jobsity.Web.Application.Users;
using Jobsity.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace Jobsity.Web
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			services
				.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddSignalR();
			services.AddTransient<IPublisher, WebPublisher>();
			services.AddTransient<IPublisher, EventPublisher>();
			services.AddRebus(configure =>
			{
				return configure
						.Logging(l => l.ColoredConsole())
						.Transport(t => t.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], "events"))
						.Routing(r => r.TypeBased().Map<MessagePostedEvent>("events"));
			});
		}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				endpoints.MapHub<ChatHub>("/hub");
			});

			app.ApplicationServices.UseRebus();
        }
    }
}
