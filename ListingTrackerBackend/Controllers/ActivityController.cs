using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListingTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly qDbContext _qDbContext;

        public ActivityController(qDbContext qDbContext)
        {
            _qDbContext = qDbContext;
        }

        [HttpPost("/getLogUploaded")]
        public async Task<IActionResult> GetLogUploaded()
        {
            var uploadedFiles = await _qDbContext.ActivityLogs.Where(s => s.LogType.Contains("Uploaded")).ToListAsync();
            return Ok(uploadedFiles);
        }

        [HttpPost("/getLogConflict")]
        public async Task<IActionResult> GetLogConflict(string id)
        {
            var uploadedFiles = await _qDbContext.ActivityLogs.Where(s => s.LogDetails.Contains(id)).ToListAsync();
            return Ok(uploadedFiles);
        }
    }
}
