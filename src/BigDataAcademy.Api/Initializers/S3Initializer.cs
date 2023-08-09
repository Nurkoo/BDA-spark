using Amazon.S3;
using Amazon.S3.Model;
using BigDataAcademy.Api.Data;

namespace BigDataAcademy.Api.Initializers;

public class S3Initializer : IServiceInitializer
{
    private const string BucketName = "big-data-academy-bucket";
    private readonly IAmazonS3 s3;

    public S3Initializer(IAmazonS3 s3)
    {
        this.s3 = s3;
    }

    public async Task Initialize()
    {
        var assembly = this.GetType().Assembly;
        await this.s3.EnsureBucketExistsAsync(BucketName);

        var existingKeys = await this.s3.GetAllObjectKeysAsync(
            BucketName,
            string.Empty,
            new Dictionary<string, object>());

        await Task.WhenAll(
            assembly
                .GetManifestResourceNames()
                .Select(o => new FileIdentifier(o))
                .Where(o => !existingKeys.Contains(o.Key))
                .Select(PutFile));

        async Task PutFile(FileIdentifier file)
        {
            var stream = assembly.GetManifestResourceStream(file.ResourceName);

            if (stream is not null)
            {
                await this.s3.PutObjectAsync(
                    new PutObjectRequest
                    {
                        BucketName = BucketName,
                        InputStream = stream,
                        Key = file.Key,
                    });
            }
        }
    }

    private class FileIdentifier
    {
        public FileIdentifier(string resourceName)
        {
            this.ResourceName = resourceName;
            this.Key = string.Join(".", resourceName.Replace($"{typeof(DataMarker).Namespace!}.", string.Empty).Split(".").SkipLast(1));
        }

        public string Key { get; }

        public string ResourceName { get; }
    }
}
