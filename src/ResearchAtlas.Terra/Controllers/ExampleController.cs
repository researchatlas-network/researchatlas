using Microsoft.AspNetCore.Mvc;

namespace ResearchAtlas.Terra.Controllers;

[ApiController]
[Route("api/example")]
public sealed class ExampleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "Workspace API is available." });
    }
}
