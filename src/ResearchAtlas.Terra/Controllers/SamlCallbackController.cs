using Microsoft.AspNetCore.Mvc;

namespace ResearchAtlas.Terra.Controllers
{
    [Route("auth/saml")]
    public class SamlCallbackController : Controller
    {
        [HttpGet("callback")]
        public IActionResult Callback()
        {
            throw new NotImplementedException();
        }
    }
}
