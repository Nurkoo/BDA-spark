using BigDataAcademy.Model.PolicyIdentifier;
using Bogus;

namespace BigDataAcademy.Model.Exposure;

public class ExposureFaker<TExposure> : Faker<TExposure>
    where TExposure : Exposure
{
    private static readonly string[] ExposureTypes =
    {
        "Vehicle", "Property", "Bodily Injury",
    };

    private int counter;

    public ExposureFaker(IReadOnlyList<Claim.Claim> claims)
    {
        this.RuleFor(
            o => o.ExposureId,
            f => f.Random.Long(100000000000, 10000000000000));

        this.RuleFor(
            o => o.ExposureType,
            f => f.PickRandom(ExposureTypes));

        this.RuleFor(
            o => o.TotalPayments,
            f => Math.Round(f.Random.Decimal(200, 50_000), 2));

        this.FinishWith((faker, exposure) =>
        {
            exposure.SourceId = faker.PickRandom(PolicyIdentifierFaker.PolicySourceSystems);
            var claim = claims[this.counter % claims.Count];
            exposure.SourceId = claim.PolicySourceSystem;
            exposure.ClaimId = claim.ClaimId;
            exposure.IngestionTime = claim.IngestionTime;
            //// if (this.counter % 100 == 1)
            //// {
            ////       exposure.SourceId = faker.PickRandom(PolicyIdentifierFaker.PolicySourceSystems);
            ////       exposure.ClaimId = faker.Random.Long(100000000000, 10000000000000);
            //// }
            //// else
            //// {
            ////     var claim = claims[this.counter];
            ////     exposure.SourceId = claim.PolicySourceSystem;
            ////     exposure.ClaimId = claim.ClaimId;
            //// }

            //// if (this.counter % 20 == 1)
            //// {
            ////     exposure.ExposureId = this.previous!.ExposureId;
            //// }

            this.counter++;
        });
    }
}
