using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Web.Controllers
{
    [Route("api/heartbeat")]
    [ApiController]
    public class HeartbeatController
    {
        [HttpGet]
        [Route(@"")]
        public IActionResult Heartbeat()
        {
            return new OkResult();
        }
    }
}
