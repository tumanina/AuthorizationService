using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Api.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        // GET api/ping
        [HttpGet]
        public string Get()
        {
            return "response";
        }
    }
}
