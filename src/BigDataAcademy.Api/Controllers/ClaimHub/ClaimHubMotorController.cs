using BigDataAcademy.Api.Authorization;
using BigDataAcademy.Api.Extensions;
using BigDataAcademy.Api.Utilities;
using BigDataAcademy.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigDataAcademy.Api.Controllers.ClaimHub;

[ApiController]
[Route("api/v1/claim-hub/motor")]
[Authorize(Policy = BasicAuthenticationPolicy.Name)]
public class ClaimHubMotorController : ControllerBase
{
    private readonly BdaPostgresContext context;
    private readonly PagedCriteria.Validator validator;

    public ClaimHubMotorController(BdaPostgresContext context, PagedCriteria.Validator validator)
    {
        this.context = context;
        this.validator = validator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Find(
        [FromQuery] PagedCriteria pagedCriteria,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await this.validator.ValidateAsync(pagedCriteria, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return Results.Json(await this.context.ClaimHubMotors.FindPaged(pagedCriteria, cancellationToken));
    }
}
