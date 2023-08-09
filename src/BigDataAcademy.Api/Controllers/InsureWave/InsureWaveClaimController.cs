using BigDataAcademy.Api.Authorization;
using BigDataAcademy.Api.Extensions;
using BigDataAcademy.Api.Utilities;
using BigDataAcademy.Model;
using BigDataAcademy.Model.Claim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigDataAcademy.Api.Controllers.InsureWave;

[ApiController]
[Authorize(Policy = ApiKeyAuthenticationPolicy.Name)]
[Route("api/v1/insure-wave/claim")]
public class InsureWaveClaimController : ControllerBase
{
    private readonly BdaPostgresContext context;
    private readonly PagedCriteria.Validator validator;
    private readonly LinkGenerator linkGenerator;

    public InsureWaveClaimController(BdaPostgresContext context, PagedCriteria.Validator validator, LinkGenerator linkGenerator)
    {
        this.context = context;
        this.validator = validator;
        this.linkGenerator = linkGenerator;
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

        var claims = await this.context
            .InsureWaveClaims
            .FindPaged(pagedCriteria, cancellationToken);

        return Results.Json(new PagedResult<ClaimResponse>
        {
            PagedCriteria = claims.PagedCriteria,
            Total = claims.Total,
            TotalPages = claims.TotalPages,
            Items = claims
                .Items
                .Select(o => new ClaimResponse
                {
                    ClaimId = o.ClaimId,
                    ProductLine = o.ProductLine,
                    State = o.State,
                    PolicyNumber = o.PolicyNumber,
                    LossDate = o.LossDate,
                    CreateTime = o.CreateTime,
                    ClaimBroker = o.ClaimBroker,
                    ClaimTier = o.ClaimTier,
                    Currency = o.Currency,
                    FaultRating = o.FaultRating,
                    HowReported = o.HowReported,
                    LossCause = o.LossCause,
                    LossType = o.LossType,
                    Situation = o.Situation,
                    PolicySourceSystem = o.PolicySourceSystem,
                    Links = new Link[]
                    {
                        new()
                        {
                            Href = this.linkGenerator.GetUriByAction(this.HttpContext, nameof(this.Get), values: new { claimId = o.ClaimId }),
                            Method = "GET",
                            Ref = "self",
                        },
                    },
                })
                .ToArray(),
        });
    }

    [HttpGet("{claimId:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get(
        long claimId,
        CancellationToken cancellationToken = default)
    {
        var item = await this.context.InsureWaveClaims.Include(o => o.Exposures).SingleOrDefaultAsync(o => o.ClaimId == claimId, cancellationToken);

        if (item is null)
        {
            return Results.NotFound();
        }

        var response = new ClaimResponse
        {
            ClaimId = item.ClaimId,
            ProductLine = item.ProductLine,
            State = item.State,
            PolicyNumber = item.PolicyNumber,
            LossDate = item.LossDate,
            CreateTime = item.CreateTime,
            ClaimBroker = item.ClaimBroker,
            ClaimTier = item.ClaimTier,
            Currency = item.Currency,
            FaultRating = item.FaultRating,
            HowReported = item.HowReported,
            LossCause = item.LossCause,
            LossType = item.LossType,
            Situation = item.Situation,
            Links = new Link[]
            {
                new()
                {
                    Href = this.linkGenerator.GetUriByAction(this.HttpContext, nameof(this.Get), values: new { claimId = item.ClaimId }),
                    Method = "GET",
                    Ref = "self",
                },
            },
            ExposureIds = item.Exposures
                .Select(e => new ExposureItem
                {
                    ExposureId = e.ExposureId,
                    Links = new Link[]
                    {
                        new()
                        {
                            Href = this.linkGenerator.GetUriByAction(this.HttpContext, nameof(this.GetExposure), values: new { claimId = e.ClaimId, exposureId = e.ExposureId }),
                            Method = "GET",
                            Ref = "self",
                        },
                    },
                }).
                ToArray(),
        };

        return Results.Json(response);
    }

    [HttpGet("{claimId:long}/exposure/{exposureId:long}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetExposure(
        long exposureId,
        CancellationToken cancellationToken = default)
    {
        var item = await this.context.InsureWaveExposures.SingleOrDefaultAsync(o => o.ExposureId == exposureId, cancellationToken);

        if (item is null)
        {
            return Results.NotFound();
        }

        return Results.Json(item);
    }
}

public class ClaimResponse : InsureWaveClaim
{
    public ExposureItem[]? ExposureIds { get; set; }

    public Link[]? Links { get; set; }
}

public class ExposureItem
{
    public long ExposureId { get; set; }

    public Link[]? Links { get; set; }
}

public class Link
{
    public string? Href { get; set; }

    public string? Ref { get; set; }

    public string? Method { get; set; }
}
