namespace BigDataAcademy.Model.Motor;

public class Motor : EntityBase
{
    public long MotorId { get; set; }

    public DateTime? LossDate { get; set; }

    public string? VehicleRegistrationNumber { get; set; }

    public DateTime? Year { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }

    public int? EngineCapacity { get; set; }

    public string? Colour { get; set; }

    public string? DamageDescription { get; set; }

    public string? TotalLostDecision { get; set; }

    public decimal? PreAccidentValue { get; set; }

    public decimal? RepairCost { get; set; }

    public int? AverageMileageForValuation { get; set; }

    public string? PolicyNumber { get; set; }

    public DateOnly? DriverDateOfBirth { get; set; }

    public string? FuelType { get; set; }

    public int? Doors { get; set; }

    public int? Seats { get; set; }

    public string? SourceId { get; set; }
}

public class ClaimHubMotor : Motor
{
}

public class InsureWaveMotor : Motor
{
}

public class ClaimZoneMotor : Motor
{
}

public class ClaimProMotor : Motor
{
}

public class ClaimWiseMotor : Motor
{
}
