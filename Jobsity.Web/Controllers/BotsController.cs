using System.Collections.Generic;
using System.Threading.Tasks;
using Jobsity.Web.Application.Bots;
using Jobsity.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Web.Controllers
{
	[Authorize]
	public class BotsController : Controller
	{
		private readonly ApplicationDbContext _dbContext;

		public BotsController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ActionResult> Index()
		{
			var bots = await _dbContext.Bots.ToListAsync();

			return View(new BotViewModel
			{
				Bots = bots
			});
		}

		public class BotViewModel
		{
			public List<Bot> Bots { get; set; }
		}
	}
}
