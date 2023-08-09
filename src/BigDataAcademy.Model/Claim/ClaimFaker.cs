using Bogus;

namespace BigDataAcademy.Model.Claim;

public partial class ClaimFaker<TClaim> : Faker<TClaim>
    where TClaim : Claim
{
    private int counter;
    private Claim? previous;

    public ClaimFaker(IReadOnlyList<PolicyIdentifier.PolicyIdentifier> identifiers, DateTime ingestionDate)
    {
        this.RuleFor(
            o => o.ClaimId,
            f => f.Random.Long(100000000000, 10000000000000));

        this.RuleFor(
            o => o.ProductLine,
            f => f.PickRandom(ProductLines).OrNull(f, 0.04f));

        this.RuleFor(
            o => o.State,
            f => f.PickRandom(States).OrNull(f, 0.04f));

        this.RuleFor(
            o => o.LossDate,
            f => f
                .Date
                .BetweenDateOnly(
                    DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
                    DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)))
                .OrNull(f, 0.05f));

        this.RuleFor(
            o => o.CreateTime,
            f => f.Date.Between(DateTime.UtcNow.AddYears(5), DateTime.UtcNow).OrNull(f, 0.05f));

        this.RuleFor(
            o => o.ClaimBroker,
            f => f.PickRandom(Brokerages).OrNull(f, 0.01f));

        this.RuleFor(
            o => o.ClaimTier,
            f => f.PickRandom(ClaimTiers).OrNull(f, 0.02f));

        this.RuleFor(
            o => o.Currency,
            f => f.PickRandom(Currencies).OrNull(f, 0.04f));

        this.RuleFor(
            o => o.FaultRating,
            f => f.PickRandom(FaultRatings).OrNull(f, 0.06f));

        this.RuleFor(
            o => o.HowReported,
            f => f.Random.WeightedRandom(HowReported, new[] { 0.55f, 0.25f, 0.10f, 0.07f, 0.03f }));

        this.RuleFor(
            o => o.LossCause,
            f => f.PickRandom(LossCauses).OrNull(f, 0.1f));

        this.RuleFor(
            o => o.LossType,
            f => f.Random.WeightedRandom(LossTypes, new[] { 0.9f, 0.05f, 0.05f }));

        this.RuleFor(
            o => o.Situation,
            f => f.PickRandom(Situations).OrNull(f, 0.05f));

        this.FinishWith((f, c) =>
        {
            var identifier = identifiers[this.counter];
            c.PolicyNumber = identifier.PolicyNumber;
            c.PolicySourceSystem = identifier.SourceSystem;
            c.IngestionTime = ingestionDate;

            //// if (this.counter % 20 == 1)
            //// {
            ////     c.ClaimId = this.previous!.ClaimId;
            //// }

            this.previous = c;
            this.counter++;
        });
    }
}
