using System;
using System.Threading.Tasks;
using Jobsity.Bots.Stock.Api.Services;
using Jobsity.Events;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Bots.Stock.Api.Controllers
{
	[ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IStooqClient _stooq;
        private readonly IJobsityClient _jobsity;
		private readonly string _commandName = "stock";

        public EventsController(IStooqClient stooq, IJobsityClient jobsity)
        {
            _stooq = stooq;
			_jobsity = jobsity;
		}

        [HttpPost]
        [Route("events")]
        public async Task<ActionResult> ProcessEvent(CommandPostedEvent @event)
        {
			if (!CanHandle(@event))
			{
				return Ok();
			}

			try
			{
				var result = await _stooq.GetStockValue(@event.CommandArguments);

				await _jobsity.NewPost(CreateSuccessMessage(@event.CommandArguments, result));
			}
			catch (Exception _)
			{
				await _jobsity.NewPost(CreateFailureMessage(@event.CommandArguments));
			}

            return Ok();
        }

		private bool CanHandle(CommandPostedEvent @event) =>
			@event.CommandName.Equals(_commandName, StringComparison.InvariantCultureIgnoreCase) &&
			!string.IsNullOrEmpty(@event.CommandArguments);

		private string CreateSuccessMessage(string stockName, decimal stockPrice) => $"{stockName} quote is ${stockPrice} per share";

		private string CreateFailureMessage(string stockName) => $"Quote for {stockName} couldn't be found";
	}
}
