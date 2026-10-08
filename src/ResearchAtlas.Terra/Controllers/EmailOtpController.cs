using Microsoft.AspNetCore.Mvc;

namespace ResearchAtlas.Terra.Controllers
{
    [Route("{tenant}/auth/email")]
    public sealed class EmailOtpController : Controller
    {
        // GET /{tenant}/auth/email
        [HttpGet]
        public IActionResult Login(
            [FromRoute] string tenant,
            [FromQuery] string? returnUrl = null)
        {
            throw new NotImplementedException();
        }

        // POST /{tenant}/auth/email
        [HttpPost]
        public IActionResult SendCode(
            [FromRoute] string tenant)
        {
            throw new NotImplementedException();
        }

        // GET /{tenant}/auth/email/verify
        [HttpGet("verify")]
        public IActionResult Verify(
            [FromRoute] string tenant)
        {
            throw new NotImplementedException();
        }

        // POST /{tenant}/auth/email/verify
        [HttpPost("verify")]
        public IActionResult VerifyCode(
            [FromRoute] string tenant)
        {
            throw new NotImplementedException();
        }
    }
}
