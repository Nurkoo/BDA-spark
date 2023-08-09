using BigDataAcademy.Model.Claim;

namespace BigDataAcademy.Model.Exposure;

public class Exposure : EntityBase
{
    public long ExposureId { get; set; }

    public long ClaimId { get; set; }

    public string? ExposureType { get; set; }

    public decimal? TotalPayments { get; set; }

    public string? SourceId { get; set; }
}

public class ClaimHubExposure : Exposure
{
}

public class InsureWaveExposure : Exposure
{
    public InsureWaveClaim Claim { get; set; } = null!;
}

public class ClaimZoneExposure : Exposure
{
    public ClaimZoneClaim Claim { get; set; } = null!;
}

public class ClaimProExposure : Exposure
{
}

public class ClaimWiseExposure : Exposure
{
}
