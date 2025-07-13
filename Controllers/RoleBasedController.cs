
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleJwtApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleBasedController : ControllerBase
    {
        // Only users with the "Admin" role can access this endpoint
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            var username = User.Identity?.Name ?? "Unknown";
            return Ok($"Hello {username}, you are authorized as Admin.");
        }

        // Any authenticated user can access this
        [Authorize]
        [HttpGet("user")]
        public IActionResult AnyAuthenticatedUser()
        {
            var username = User.Identity?.Name ?? "Unknown";
            var roles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                                   .Select(c => c.Value);

            return Ok($"Hello {username}, your roles: {string.Join(", ", roles)}");
        }
    }
}
