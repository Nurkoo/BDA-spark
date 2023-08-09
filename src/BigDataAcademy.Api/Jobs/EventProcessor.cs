using System.Text.Json;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using BigDataAcademy.Model;
using BigDataAcademy.Model.Claim;
using BigDataAcademy.Model.Event;
using BigDataAcademy.Model.Exposure;
using BigDataAcademy.Model.Motor;
using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace BigDataAcademy.Api.Jobs;

public class EventProcessor : IJob
{
    private readonly BdaPostgresContext context;
    private readonly IAmazonKinesis kinesis;

    public EventProcessor(BdaPostgresContext context, IAmazonKinesis kinesis)
    {
        this.context = context;
        this.kinesis = kinesis;
    }

    public string CronExpression => "*/5 * * * *";

    [SkipWhenPreviousJobIsRunning]
    [DisableConcurrentExecution(10)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var events = this.context.Events
            .Where(o => !o.Processed && o.ProcessAt < DateTime.UtcNow)
            .OrderBy(o => o.ProcessAt)
            .ThenBy(o => o.Type.ToLower().Contains("exposure"))
            .Take(1000)
            .ToList();

        while (events.Any())
        {
            foreach (var @event in events)
            {
                @event.Processed = true;
                var type = typeof(IntegrationEvent).Assembly.GetType(@event.Type)!;
                var obj = JsonSerializer.Deserialize(@event.Body, type)!;

                if (type.Name.ToLower().Contains("claimwise"))
                {
                    var streamName = type.Name switch
                    {
                        nameof(ClaimWiseClaim) => "claim-wise-claims",
                        nameof(ClaimWiseExposure) => "claim-wise-exposure",
                        nameof(ClaimWiseMotor) => "claim-wise-motor",
                        _ => throw new ArgumentException(type.Name),
                    };

#pragma warning disable CS4014
                    this.kinesis.PutRecordAsync(
#pragma warning restore CS4014
                        new PutRecordRequest
                        {
                            StreamName = streamName,
                            PartitionKey = "1",
                            Data = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(obj)),
                        },
                        cancellationToken);
                }
                else
                {
                    var item = this.context.Model.FindEntityType(type);
                    var key = item!.FindPrimaryKey()!.Properties.Single().Name;
                    var current = this.context.Find(type, obj.GetType().GetProperty(key)!.GetValue(obj, null));

                    if (current == null)
                    {
                        this.context.Add(obj);
                    }
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);
            events = this.context.Events
                .Where(o => !o.Processed && o.ProcessAt < DateTime.UtcNow)
                .OrderBy(o => o.ProcessAt)
                .ThenBy(o => o.Type.ToLower().Contains("exposure"))
                .Take(1000)
                .ToList();
        }
    }

    public class SkipWhenPreviousJobIsRunningAttribute : JobFilterAttribute, IClientFilter, IApplyStateFilter
    {
        public void OnCreating(CreatingContext context)
        {
            var connection = context.Connection as JobStorageConnection;

            // We can't handle old storages
            if (connection == null)
            {
                return;
            }

            // We should run this filter only for background jobs based on
            // recurring ones
            if (!context.Parameters.ContainsKey("RecurringJobId"))
            {
                return;
            }

            var recurringJobId = context.Parameters["RecurringJobId"] as string;

            // RecurringJobId is malformed. This should not happen, but anyway.
            if (string.IsNullOrWhiteSpace(recurringJobId))
            {
                return;
            }

            var running = connection.GetValueFromHash($"recurring-job:{recurringJobId}", "Running");

            if ("yes".Equals(running, StringComparison.OrdinalIgnoreCase))
            {
                context.Canceled = true;
            }
        }

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState is EnqueuedState)
            {
                var recurringJobId =
                    SerializationHelper.Deserialize<string>(
                        context.Connection.GetJobParameter(context.BackgroundJob.Id, "RecurringJobId"));

                if (string.IsNullOrWhiteSpace(recurringJobId))
                {
                    return;
                }

                transaction.SetRangeInHash(
                    $"recurring-job:{recurringJobId}",
                    new[] { new KeyValuePair<string, string>("Running", "yes") });
            }
            else if (context.NewState.IsFinal /* || context.NewState is FailedState*/)
            {
                var recurringJobId =
                    SerializationHelper.Deserialize<string>(
                        context.Connection.GetJobParameter(context.BackgroundJob.Id, "RecurringJobId"));

                if (string.IsNullOrWhiteSpace(recurringJobId))
                {
                    return;
                }

                transaction.SetRangeInHash(
                    $"recurring-job:{recurringJobId}",
                    new[] { new KeyValuePair<string, string>("Running", "no") });
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}
