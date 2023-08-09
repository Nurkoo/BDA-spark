using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigDataAcademy.Api.Controllers;

[ApiController]
[Route("api/v1/hello")]
[AllowAnonymous]
public class HelloController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Find(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(Results.Content("Hello!"));
    }
}
