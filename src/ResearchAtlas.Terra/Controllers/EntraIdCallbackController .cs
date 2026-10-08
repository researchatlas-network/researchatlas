using Microsoft.AspNetCore.Mvc;

namespace ResearchAtlas.Terra.Controllers
{
    [Route("auth/entra")]
    public class EntraIdCallbackController : Controller
    {
        [HttpGet("callback")]
        public IActionResult Callback()
        {
            throw new NotImplementedException();
        }
    }
}
