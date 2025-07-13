
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleJwtApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PolicyBasedController : ControllerBase
    {
        [Authorize(Policy = "MustBeAdmin")]
        [HttpGet("admin-policy")]
        public IActionResult AdminPolicy()
        {
            var username = User.Identity?.Name ?? "Unknown";
            return Ok($"Hello {username}, you passed the Admin policy.");
        }

        [Authorize(Policy = "MustBeManager")]
        [HttpGet("manager-policy")]
        public IActionResult ManagerPolicy()
        {
            var username = User.Identity?.Name ?? "Unknown";
            return Ok($"Hello {username}, you passed the Manager policy based on claims.");
        }
    }
}
