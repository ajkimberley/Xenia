using Microsoft.AspNetCore.Mvc;

namespace ScreenMedia.Xenia.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HelloController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public IActionResult Get() => Ok("Hello Xenia");
}
