import boto3
import json

# endpoint_url = "http://localhost.localstack.cloud:4566"
endpoint_url = "http://localhost:4566"
# endpoint_url = "http://localstack:4566"
# alternatively, to use HTTPS endpoint on port 443:
# endpoint_url = "https://localhost.localstack.cloud"


def main():
    client = boto3.client(
        "s3",
        endpoint_url=endpoint_url,
        region_name="eu-west-1",
        aws_access_key_id="fake_key",
        aws_secret_access_key="fake_key",
    )

    # Creating the S3 bucket

    client.create_bucket(
        Bucket="test-bucket",
        CreateBucketConfiguration={"LocationConstraint": "eu-west-1"},
    )

    # Listing bucekts (to validate if it was created successfully)

    result = client.list_buckets()
    print(result.get("Buckets"))

    # Writing data to s3 bucket
    more_binary_data = b"Here we have some more data"

    sample_json_data = {
        "key1": "value1",
        "key2": "value2",
        "key3": {
            "keyval1": "valval1",
            "keywal2": "valval2",
        },
        "key4": ["listval1", "listval2", "listval3"],
    }

    client.put_object(
        # Body=json.dumps(sample_json_data),
        Body=more_binary_data,
        Bucket="test-bucket",
        Key="data/insurwave/test_data.txt",
    )

    for key in client.list_objects(Bucket="test-bucket")["Contents"]:
        print(key["Key"])

        data = client.get_object(Bucket="test-bucket", Key=key.get("Key"))
        contents = data["Body"].read()
        print(contents.decode("utf-8"))


if __name__ == "__main__":
    main()
