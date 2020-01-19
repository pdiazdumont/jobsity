using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jobsity.Bots.Stock.Api.Services;
using Jobsity.Events.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Bots.Stock.Api.Controllers
{
	[ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IStooqClient _stooq;
		private readonly Regex _commandRegex = new Regex(@"\/stock=([^=& ]*)");

        public EventsController(IStooqClient stooq)
        {
            _stooq = stooq;
        }

        [HttpPost]
        [Route("events")]
        public async Task<ActionResult> ProcessEvent(MessagePostedEvent @event)
        {
			if (!CanHandle(@event))
			{
				return Ok();
			}

			var matches = _commandRegex.Match(@event.Text);

			try
			{
				var result = await _stooq.GetStockValue(matches.Groups[1].ToString());


			}
			catch (Exception _)
			{

			}

            return Ok();
        }

		private bool CanHandle(MessagePostedEvent @event) => _commandRegex.IsMatch(@event.Text);

		private string CreateSuccessMessage(string stockName, long stockPrice) => $"{stockName} quote is ${stockPrice} per share";

		private string CreateFailureMessage(string stockName) => $"Quote for {stockName} couldn't be found";
	}
}
