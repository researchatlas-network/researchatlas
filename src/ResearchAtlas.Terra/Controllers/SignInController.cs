using Microsoft.AspNetCore.Mvc;

namespace ResearchAtlas.Terra.Controllers
{
    [Route("{tenant}/auth")]
    public class SignInController: Controller
    {
        // GET /{tenant}/auth/login
        [HttpGet("login")]
        public IActionResult Login(
            [FromRoute] string tenant,
            [FromQuery] string? returnUrl = null)
        {
            throw new NotImplementedException();
        }

        // POST /{tenant}/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout(
            [FromRoute] string tenant)
        {
            throw new NotImplementedException();
        }
    }
}
