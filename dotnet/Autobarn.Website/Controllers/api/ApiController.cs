using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class DiscoveryEndpointController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            var welcome = new {
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    }
                },
                message = "Welcome to the Autobarn API",
            };
            return Ok(welcome);
        }
    }
}
