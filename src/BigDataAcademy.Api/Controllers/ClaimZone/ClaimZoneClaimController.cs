using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;
using BigDataAcademy.Api.Authorization;
using BigDataAcademy.Api.Extensions;
using BigDataAcademy.Api.Utilities;
using BigDataAcademy.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigDataAcademy.Api.Controllers.ClaimZone;

[ApiController]
[Authorize(Policy = OAuthAuthenticationPolicy.Name)]
[Route("api/v1/claim-zone/claim")]
public class ClaimZoneClaimController : ControllerBase
{
    private readonly BdaPostgresContext context;
    private readonly ClaimRequest.Validator claimValidator;
    private readonly PagedCriteria.Validator pagedCriteriaValidator;

    public ClaimZoneClaimController(BdaPostgresContext context, ClaimRequest.Validator claimValidator, PagedCriteria.Validator pagedCriteriaValidator)
    {
        this.context = context;
        this.claimValidator = claimValidator;
        this.pagedCriteriaValidator = pagedCriteriaValidator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Find(
        [FromQuery] PagedCriteria pagedCriteria,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await this.pagedCriteriaValidator.ValidateAsync(pagedCriteria, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var claims = await this.context
            .ClaimZoneClaims
            .FindPaged(pagedCriteria, cancellationToken);

        return Results.Json(claims);
    }

    [HttpPost("{claimId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/xml", "application/json")]
    public async Task<IResult> Get(
        long claimId,
        [FromHeader(Name = "X-BDA-CORRELATION-ID"), Required] string correlationId,
        [FromBody] Request claimRequest,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await this.claimValidator.ValidateAsync(new ClaimRequest { CorrelationId = correlationId }, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var query = this.context.ClaimZoneClaims.AsQueryable();

        if (claimRequest.IncludeDetails)
        {
            query = query.Include(o => o.Exposures);
        }

        var item = await query.SingleOrDefaultAsync(o => o.ClaimId == claimId, cancellationToken);

        if (item is null)
        {
            return Results.NotFound();
        }

        var response = new Response
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
            Exposures = item.Exposures
                .Select(o => new Item
                {
                    ClaimId = o.ClaimId,
                    ExposureId = o.ExposureId,
                    ExposureType = o.ExposureType,
                    TotalPayments = o.TotalPayments,
                })
                .ToArray(),
        };

        if (!this.Request.Headers.Accept.Contains("application/json"))
        {
            var xmlSerializer = new XmlSerializer(typeof(Response));
            string xml;

            await using (var sww = new StringWriter())
            {
                await using (var writer = XmlWriter.Create(sww))
                {
                    xmlSerializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            return Results.Content(xml, "application/xml");
        }

        return Results.Json(response);
    }
}

public class Request
{
    [FromBody]
    public bool IncludeDetails { get; set; } = false;
}

public class ClaimRequest
{
    public string CorrelationId { get; set; } = null!;

    public class Validator : AbstractValidator<ClaimRequest>
    {
        public Validator()
        {
            this.RuleFor(o => o.CorrelationId)
                .NotEmpty();
        }
    }
}

public class Response
{
    public long ClaimId { get; set; }

    public string? ProductLine { get; set; }

    public string? State { get; set; }

    public string? PolicyNumber { get; set; }

    public DateOnly? LossDate { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? ClaimBroker { get; set; }

    public string? ClaimTier { get; set; }

    public string? Currency { get; set; }

    public string? FaultRating { get; set; }

    public string? HowReported { get; set; }

    public string? LossCause { get; set; }

    public string? LossType { get; set; }

    public string? Situation { get; set; }

    public Item[] Exposures { get; set; } = null!;
}

public class Item
{
    public long ExposureId { get; set; }

    public long ClaimId { get; set; }

    public string? ExposureType { get; set; }

    public decimal? TotalPayments { get; set; }

    public string? SourceId { get; set; }
}
