using System.Text.Json;
using System.Text.Json.Serialization;
using BigDataAcademy.Model;
using BigDataAcademy.Model.Claim;
using BigDataAcademy.Model.Event;
using BigDataAcademy.Model.Exposure;
using BigDataAcademy.Model.Motor;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace BigDataAcademy.Api.Jobs;

public class Seeder : IJob
{
    private readonly BdaPostgresContext context;

    public Seeder(BdaPostgresContext context)
    {
        this.context = context;
    }

    public string CronExpression => Cron.Never();

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        await this.context.Database.ExecuteSqlRawAsync("DELETE FROM integration_event", cancellationToken: cancellationToken);

        var sourceSystems = new[] { "claimhub", "insurwave", "claimzone", "claimpro", "claimwise" };
        foreach (var sourceSystem in sourceSystems)
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            var fileName = Path.Combine(root, @$"{sourceSystem}.json");

            if (File.Exists(fileName))
            {
                await using var file = File.OpenRead(fileName);
                var data = JsonSerializer.Deserialize<Helper>(file)!;
                var events = data
                    .MotorPolicies
                    .Select(o => new IntegrationEvent
                    {
                        Body = JsonSerializer.Serialize(o),
                        Processed = false,
                        Type = sourceSystem switch
                        {
                            "claimhub" => typeof(ClaimHubMotor).FullName!,
                            "insurwave" => typeof(InsureWaveMotor).FullName!,
                            "claimzone" => typeof(ClaimZoneMotor).FullName!,
                            "claimpro" => typeof(ClaimProMotor).FullName!,
                            "claimwise" => typeof(ClaimWiseMotor).FullName!,
                            _ => throw new ArgumentException(sourceSystem),
                        },
                        ProcessAt = o.IngestionTime,
                    })
                    .Concat(data
                        .Claims
                        .Select(o => new IntegrationEvent
                        {
                            Body = JsonSerializer.Serialize(o),
                            Processed = false,
                            Type = sourceSystem switch
                            {
                                "claimhub" => typeof(ClaimHubClaim).FullName!,
                                "insurwave" => typeof(InsureWaveClaim).FullName!,
                                "claimzone" => typeof(ClaimZoneClaim).FullName!,
                                "claimpro" => typeof(ClaimProClaim).FullName!,
                                "claimwise" => typeof(ClaimWiseExposure).FullName!,
                                _ => throw new ArgumentException(sourceSystem),
                            },
                            ProcessAt = o.IngestionTime,
                        })
                        .ToList())
                    .Concat(data
                        .Exposures
                        .Select(o => new IntegrationEvent
                        {
                            Body = JsonSerializer.Serialize(o),
                            Processed = false,
                            Type = sourceSystem switch
                            {
                                "claimhub" => typeof(ClaimHubExposure).FullName!,
                                "insurwave" => typeof(InsureWaveExposure).FullName!,
                                "claimzone" => typeof(ClaimZoneExposure).FullName!,
                                "claimpro" => typeof(ClaimProExposure).FullName!,
                                "claimwise" => typeof(ClaimWiseExposure).FullName!,
                                _ => throw new ArgumentException(sourceSystem),
                            },
                            ProcessAt = o.IngestionTime,
                        })
                        .ToList())
                    .ToList();

                foreach (var @event in events)
                {
                    this.context.Add(@event);
                }

                await this.context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

public class Helper
{
    [JsonPropertyName("claims")]
    public Claim[] Claims { get; set; } = null!;

    [JsonPropertyName("exposures")]
    public Exposure[] Exposures { get; set; } = null!;

    [JsonPropertyName("motorPolicies")]
    public Motor[] MotorPolicies { get; set; } = null!;
}
