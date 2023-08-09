using System.Text.Json;
using BigDataAcademy.Model.Claim;
using BigDataAcademy.Model.Exposure;
using BigDataAcademy.Model.Motor;
using BigDataAcademy.Model.PolicyIdentifier;

var root = Path.Combine(Directory.GetCurrentDirectory(), "data");
if (!Directory.Exists(root))
{
    Directory.CreateDirectory(root);
}

foreach (var sourceSystem in new[] { "claimhub", "insurwave", "claimzone", "claimpro", "claimwise", "claimify" })
{
    var claims = new List<Claim>();
    var exposures = new List<Exposure>();
    var motorPolicies = new List<Motor>();
    foreach (var i in Enumerable.Range(0, 36).ToList())
    {
        var policyIdentifierFaker = new PolicyIdentifierFaker(sourceSystem);
        var policyIdentifierPool = policyIdentifierFaker.Generate(200);
        var motorFaker = new MotorFaker<Motor>(policyIdentifierPool, DateTime.UtcNow.AddDays(-6 + i));
        motorPolicies.AddRange(motorFaker.Generate(200));
        var claimFaker = new ClaimFaker<Claim>(policyIdentifierPool, DateTime.UtcNow.AddDays(-6 + i));
        var claimsLocal = claimFaker.Generate(200);
        claims.AddRange(claimsLocal);
        var exposureFaker = new ExposureFaker<Exposure>(claimsLocal);
        exposures.AddRange(exposureFaker.Generate(300));
    }

    if (sourceSystem != "claimify")
    {
        WriteJson(sourceSystem, new { claims, exposures, motorPolicies });
    }
    else
    {
        WriteCsv(claims, exposures, motorPolicies);
    }
}

void WriteJson(string sourceSystem, object data)
{
    var filePath = Path.Combine(root, $@"{sourceSystem}.json");
    var jsonString = JsonSerializer.Serialize(data);
    File.WriteAllText(filePath, jsonString);
}

void WriteCsv(List<Claim> claims, List<Exposure> exposures, List<Motor> motors)
{
    using var claimsWriter = new StreamWriter(Path.Combine(root, "claimify-claims.csv"));
    claimsWriter.WriteLine("cdi,pl,lo,pn,ld,t,cb,ct,c,fr,hr,lc,lt,situation,id");
    foreach (var claim in claims)
    {
        claimsWriter.WriteLine(
            $"{WriteCsvValue(claim.ClaimId)}," +
            $"{WriteCsvValue(claim.ProductLine)}," +
            $"{WriteCsvValue(claim.State)}," +
            $"{WriteCsvValue(claim.PolicyNumber)}," +
            $"{WriteCsvValue(claim.LossDate)}," +
            $"{WriteCsvValue(claim.CreateTime)}," +
            $"{WriteCsvValue(claim.ClaimBroker)}," +
            $"{WriteCsvValue(claim.ClaimTier)}," +
            $"{WriteCsvValue(claim.Currency)}," +
            $"{WriteCsvValue(claim.FaultRating)}," +
            $"{WriteCsvValue(claim.HowReported)}," +
            $"{WriteCsvValue(claim.LossCause)}," +
            $"{WriteCsvValue(claim.LossType)}," +
            $"{WriteCsvValue(claim.Situation)}," +
            $"{WriteCsvValue(claim.IngestionTime)}");
    }

    using var exposureWriter = new StreamWriter(Path.Combine(root, "claimify-exposure.csv"));
    exposureWriter.WriteLine("eid,cid,et,tp,id");
    foreach (var exposure in exposures)
    {
        exposureWriter.WriteLine(
            $"{WriteCsvValue(exposure.ExposureId)}," +
            $"{WriteCsvValue(exposure.ClaimId)}," +
            $"{WriteCsvValue(exposure.ExposureType)}," +
            $"{WriteCsvValue(exposure.TotalPayments)}," +
            $"{WriteCsvValue(exposure.IngestionTime)}");
    }

    using var motorWriter = new StreamWriter(Path.Combine(root, "claimify-motor.csv"));

    motorWriter.WriteLine("mid,ld,vrn,y,m,mo,ec,col,dd,tld,pav,rc,amfv,pn,ddob,ft,d,s,id");

    foreach (var motorPolicy in motors)
    {
        motorWriter.WriteLine(
            $"{WriteCsvValue(motorPolicy.MotorId)}," +
            $"{WriteCsvValue(motorPolicy.LossDate)}," +
            $"{WriteCsvValue(motorPolicy.VehicleRegistrationNumber)}," +
            $"{WriteCsvValue(motorPolicy.Year)}," +
            $"{WriteCsvValue(motorPolicy.Make)}," +
            $"{WriteCsvValue(motorPolicy.Model)}," +
            $"{WriteCsvValue(motorPolicy.EngineCapacity)}," +
            $"{WriteCsvValue(motorPolicy.Colour)}," +
            $"{WriteCsvValue(motorPolicy.DamageDescription)}," +
            $"{WriteCsvValue(motorPolicy.TotalLostDecision)}," +
            $"{WriteCsvValue(motorPolicy.PreAccidentValue)}," +
            $"{WriteCsvValue(motorPolicy.RepairCost)}," +
            $"{WriteCsvValue(motorPolicy.AverageMileageForValuation)}," +
            $"{WriteCsvValue(motorPolicy.PolicyNumber)}," +
            $"{WriteCsvValue(motorPolicy.DriverDateOfBirth)}," +
            $"{WriteCsvValue(motorPolicy.FuelType)}," +
            $"{WriteCsvValue(motorPolicy.Doors)}," +
            $"{WriteCsvValue(motorPolicy.Seats)}," +
            $"{WriteCsvValue(motorPolicy.IngestionTime)}");
    }
}

string WriteCsvValue(object? o)
{
    return $"\"{o}\"";
}
