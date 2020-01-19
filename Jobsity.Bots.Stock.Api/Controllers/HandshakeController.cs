using Jobsity.Bots.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Bots.Stock.Api.Controllers
{
    [ApiController]
    public class HandshakeController : ControllerBase
    {
        [HttpPost]
        [Route("handshake")]
        public ActionResult Handshake([FromBody] HandshakeRequest request)
        {
            if (ModelState.IsValid && request.Type == "url_verification")
            {
                return Ok(request.Challenge);
            }

            return BadRequest();
        }
    }
}
