using Amazon.Kinesis;
using Amazon.Kinesis.Model;

namespace BigDataAcademy.Api.Initializers;

public class KinesisInitializer : IServiceInitializer
{
    private static readonly IEnumerable<string> Streams = new[]
    {
        "claim-wise-claims",
        "claim-wise-exposure",
        "claim-wise-motor",
    };

    private readonly IAmazonKinesis kinesis;

    public KinesisInitializer(IAmazonKinesis kinesis)
    {
        this.kinesis = kinesis;
    }

    public async Task Initialize()
    {
        var response = await this.kinesis.ListStreamsAsync();
        var existingStreams = response.StreamNames.ToList();

        while (response.HasMoreStreams)
        {
            existingStreams.AddRange(response.StreamNames);
            response = await this.kinesis.ListStreamsAsync();
        }

        foreach (var streamName in Streams)
        {
            if (!existingStreams.Contains(streamName))
            {
                await this.kinesis.CreateStreamAsync(
                    new CreateStreamRequest
                    {
                        ShardCount = 1,
                        StreamName = streamName,
                        StreamModeDetails = new StreamModeDetails
                        {
                            StreamMode = StreamMode.ON_DEMAND,
                        },
                    });
            }
        }
    }
}
