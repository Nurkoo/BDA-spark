from pyspark.sql import SparkSession

# spark = SparkSession.builder.appName("Sample app").getOrCreate()

# rdd = spark.sparkContext.parallelize(range(1, 100))

# print(f"THE SUM IS HERE: {rdd.sum()}")

spark = SparkSession.builder.appName("my_app").getOrCreate()

spark._jsc.hadoopConfiguration().set("fs.s3a.access.key", "mykey")
spark._jsc.hadoopConfiguration().set("fs.s3a.secret.key", "mysecret")
spark._jsc.hadoopConfiguration().set("fs.s3a.path.style.access", "true")
spark._jsc.hadoopConfiguration().set(
    "fs.s3a.impl", "org.apache.hadoop.fs.s3a.S3AFileSystem"
)
spark._jsc.hadoopConfiguration().set("com.amazonaws.services.s3.enableV4", "true")
spark._jsc.hadoopConfiguration().set(
    "fs.s3a.aws.credentials.provider",
    "org.apache.hadoop.fs.s3a.SimpleAWSCredentialsProvider",
)
spark._jsc.hadoopConfiguration().set("fs.s3a.endpoint", "http://localstack:4566")


data = [(12, 23, 34), (23, 34, 45), (34, 45, 45), (34, 45, 56)]
schema = ["Pierwsza", "Druga", "Trzecia"]


df = spark.createDataFrame(data, schema=schema)

df.write.parquet("s3a://test-bucket/parquet/people.parquet")
