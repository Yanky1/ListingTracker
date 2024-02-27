using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ListingTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        // endpoint for getting list of AcceptedPersonWithMatchingRecords
        [HttpGet("GetAcceptedPersonWithMatchingRecords")]
        public IActionResult GetAcceptedPersonWithMatchingRecords()
        {
            return Ok();
        }
    }
}
