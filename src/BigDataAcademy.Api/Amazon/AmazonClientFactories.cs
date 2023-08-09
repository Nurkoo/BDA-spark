using Amazon;
using Amazon.Kinesis;
using Amazon.S3;

namespace BigDataAcademy.Api.Amazon;

public static class AmazonClientFactories
{
    public static IAmazonS3 ProvideS3(IServiceProvider serviceProvider)
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ForcePathStyle = true,
            ServiceURL = serviceProvider.GetRequiredService<AppSettings>().Aws.S3,
        };

        return new AmazonS3Client("test", "test", config);
    }

    public static IAmazonKinesis ProvideKinesis(IServiceProvider serviceProvider)
    {
        var config = new AmazonKinesisConfig
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ServiceURL = serviceProvider.GetRequiredService<AppSettings>().Aws.Kinesis,
        };

        return new AmazonKinesisClient("test", "test", config);
    }
}
