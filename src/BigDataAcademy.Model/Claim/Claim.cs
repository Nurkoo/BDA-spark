using BigDataAcademy.Model.Exposure;

namespace BigDataAcademy.Model.Claim;

public class Claim : EntityBase
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

    public string? PolicySourceSystem { get; set; }
}

public class ClaimHubClaim : Claim
{
}

public class InsureWaveClaim : Claim
{
    public ICollection<InsureWaveExposure> Exposures { get; set; } = null!;
}

public class ClaimZoneClaim : Claim
{
    public ICollection<ClaimZoneExposure> Exposures { get; set; } = null!;
}

public class ClaimProClaim : Claim
{
}

public class ClaimWiseClaim : Claim
{
}
