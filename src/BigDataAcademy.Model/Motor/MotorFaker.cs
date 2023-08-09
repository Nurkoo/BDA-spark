using Bogus;

namespace BigDataAcademy.Model.Motor;

public partial class MotorFaker<TMotor> : Faker<TMotor>
    where TMotor : Motor
{
    private int counter;
    private Motor? previous;

    public MotorFaker(IReadOnlyList<PolicyIdentifier.PolicyIdentifier> identifiers, DateTime ingestionDate)
    {
        this.RuleFor(
            o => o.MotorId,
            f => f.Random.Long(100000000000, 10000000000000));

        this.RuleFor(
            o => o.LossDate,
            f => f
                .Date
                .Between(
                    DateTime.UtcNow.AddYears(-5),
                    DateTime.UtcNow.AddMonths(-2))
                .OrNull(f, 0.07f));

        this.RuleFor(
            o => o.VehicleRegistrationNumber,
            f => f.Random.Replace("??## ???").OrNull(f, 0.03f));

        this.RuleFor(
            o => o.Year,
            f => f
                .Date
                .Between(
                    DateTime.UtcNow.AddYears(-20),
                    DateTime.UtcNow.AddMonths(-10)));

        this.RuleFor(
            o => o.Make,
            f => f.Vehicle.Manufacturer());

        this.RuleFor(
            o => o.Model,
            f => f.Vehicle.Model());

        this.RuleFor(
            o => o.EngineCapacity,
            f => f.Random.Int(900, 7_000));

        this.RuleFor(
            o => o.Colour,
            f => f.PickRandom(Colours));

        this.RuleFor(
            o => o.DamageDescription,
            f => f.PickRandom(DamageDescriptions).OrNull(f, 0.01f));

        this.RuleFor(
            o => o.TotalLostDecision,
            f => f.PickRandom(TotalLossDecisions));

        this.RuleFor(
            o => o.PreAccidentValue,
            f => Math.Round(f.Random.Decimal(5000, 50_000), 2).OrNull(f, 0.03f));

        this.RuleFor(
            o => o.RepairCost,
            f => Math.Round(f.Random.Decimal(10, 50_000), 2).OrNull(f, 0.06f));

        this.RuleFor(
            o => o.AverageMileageForValuation,
            f => f.Random.Int(1_000, 200_000).OrNull(f, 0.06f));

        this.RuleFor(
            o => o.DriverDateOfBirth,
            f => f
                .Date
                .BetweenDateOnly(
                    DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-70)),
                    DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18)))
                .OrNull(f, 0.08f));

        this.RuleFor(
            o => o.FuelType,
            f => f.PickRandom(FuelTypes).OrNull(f, 0.01f));

        this.RuleFor(
            o => o.Doors,
            f => f.PickRandom(Doors).OrNull(f, 0.05f));

        this.RuleFor(
            o => o.Seats,
            f => f.PickRandom(Seats).OrNull(f, 0.04f));

        this.FinishWith((f, m) =>
        {
            var identifier = identifiers[this.counter];
            m.PolicyNumber = identifier.PolicyNumber;
            m.SourceId = identifier.SourceSystem;
            m.IngestionTime = ingestionDate;
            ////if (this.counter % 20 == 1)
            ////{
            ////    m.MotorId = this.previous!.MotorId;
            ////}

            this.previous = m;
            this.counter++;
        });
    }
}
