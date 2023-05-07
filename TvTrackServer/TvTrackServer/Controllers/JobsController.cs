using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace TvTrackServer.Controllers
{
    [Route("jobs")]
    [ApiController]
    public class JobsController : CustomControllerBase
    {
        private readonly ISchedulerFactory _factory;

        public JobsController(TvTrackServerDbContext dbContext, ISchedulerFactory factory) : base(dbContext)
        {
            _factory = factory;
        }

        [HttpGet("run")]
        public async Task<IActionResult> Run()
        {
            IScheduler scheduler = await _factory.GetScheduler();

            await scheduler.TriggerJob(new JobKey("Daily Notification"));

            await scheduler.TriggerJob(new JobKey("Daily Calendar Synchronization"));

            return Ok();
        }
    }
}
