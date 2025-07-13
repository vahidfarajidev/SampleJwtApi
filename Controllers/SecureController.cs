
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleJwtApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecureController : ControllerBase
    {
        [Authorize]
        [HttpGet("protected")]
        public IActionResult GetProtectedData()
        {
            var username = User.Identity?.Name ?? "Unknown";
            return Ok($"Welcome {username}, you have accessed a protected endpoint!");
        }
    }
}
