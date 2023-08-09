using BigDataAcademy.Api.Authorization;
using BigDataAcademy.Api.Extensions;
using BigDataAcademy.Api.Utilities;
using BigDataAcademy.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigDataAcademy.Api.Controllers.ClaimZone;

[ApiController]
[Route("api/v1/claim-zone/motor")]
[Authorize(Policy = ApiKeyAuthenticationPolicy.Name)]
public class ClaimZoneMotorController : ControllerBase
{
    private readonly BdaPostgresContext context;
    private readonly PagedCriteria.Validator validator;

    public ClaimZoneMotorController(BdaPostgresContext context, PagedCriteria.Validator validator)
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

        return Results.Json(await this.context.ClaimZoneMotors.FindPaged(pagedCriteria, cancellationToken));
    }
}
