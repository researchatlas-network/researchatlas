using Microsoft.AspNetCore.Mvc;

namespace ResearchAtlas.Terra.Controllers
{
    [Route("auth/oidc")]
    public class OidcCallbackController : Controller
    {
        [HttpGet("callback")]
        public IActionResult Callback()
        {
            throw new NotImplementedException();
        }
    }
}
