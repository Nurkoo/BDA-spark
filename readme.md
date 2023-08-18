Tasks:


Stage 0:

    * Prepare the script that creates s3 bucket called "bda-data-bucket" (mocked by localsttack - available at https://localhost:4566)
    * Upload sample .txt file to the bucket and verify that it's working
    * Useful links:
        * https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/s3.html
        * https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/s3/client/create_bucket.html
        * https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/s3/client/put_object.html
    * Localstack credentials:
        * region_name="eu-west-1",
        * aws_access_key_id="fake_key",
        * aws_secret_access_key="fake_key",
        * endpoint="http://localhost:4566" (if accessed from local machine),
        * endpoint="http://localstack:4566" (if accessed from another container ie spark container),
    * View bucket contents from the web browser:
        * http://localhost:4566/<bucket_name>


Stage 1:

    * Change the implementation of API reader for claim-hub
    * Each file should be written in AWS S3 (mocked by localsttack - available at https://localhost:4566)
    * s3://bda-data-bucket/claim-hub/<execution_date>/<table_name>_part.json


Stage 2:

    * Change the implementation of API reader for insure-wave
    * Each file should be written in AWS S3 (mocked by localsttack - available at https://localhost:4566)
    * s3://bda-data-bucket/insure-wave/<execution_date>/<table_name>_part.json


Stage 3:

    * Change the implementation of API reader for claim-zone
    * Each file should be written in AWS S3 (mocked by localsttack - available at https://localhost:4566)
    * s3://bda-data-bucket/claim-zone/<execution_date>/<table_name>.json


SPARK AWS SETTINGS

spark._jsc.hadoopConfiguration().set("fs.s3a.access.key", "mykey")
spark._jsc.hadoopConfiguration().set("fs.s3a.secret.key", "mysecret")
spark._jsc.hadoopConfiguration().set("fs.s3a.path.style.access", "true")
spark._jsc.hadoopConfiguration().set("fs.s3a.impl", "org.apache.hadoop.fs.s3a.S3AFileSystem")
spark._jsc.hadoopConfiguration().set("com.amazonaws.services.s3.enableV4", "true")
spark._jsc.hadoopConfiguration().set("fs.s3a.aws.credentials.provider","org.apache.hadoop.fs.s3a.SimpleAWSCredentialsProvider",)
spark._jsc.hadoopConfiguration().set("fs.s3a.endpoint", "http://localstack:4566")


Stage 4:

    * Create a Spark job that reads all data for a claim table from three sources (AWS S3 paths) for a single day
    * The data should be read from each view from 5 parts saved in previous step
    * The date for which the data should be retrieved should be passed as an argument with spark-submit
    * Combine all parts and all views into one table
    * Remove duplicates
    * Add another column called "exec_date" and fill it with the date which was read from the AWS S3
    * Save final table in postgres


Stage 5:

    * Create a Spark job that reads all data for a motor table from three sources (AWS S3 paths) for a single day
    * The data should be read from each view from 5 parts saved in previous step
    * The date for which the data should be retrieved should be passed as an argument with spark-submit
    * Combine all parts and all views into one table
    * Remove duplicates
    * Add another column called "exec_date" and fill it with the date which was read from the AWS S3
    * Save final table in postgres


Stage 6:

    * Create a Spark job that reads all data for a exposure table from three sources (AWS S3 paths) for a single day
    * The data should be read from each view from 5 parts saved in previous step
    * The date for which the data should be retrieved should be passed as an argument with spark-submit
    * Combine all parts and all views into one table
    * Remove duplicates
    * Add another column called "exec_date" and fill it with the date which was read from the AWS S3
    * Save final table in postgres


Stage 7:
    * Create simple DAG with EmptyOperatos and PythonOperators
    * Start (emptyOperator) -> print_date (PythonOperator), print_folder_content (PythonOperator) -> end (emptyOperator)
    * print_date - prints current date
    * print_folder_content - prints contents of the current folder
    * Tasks print_date and print_folder_content should be in one TaskGroup and executed in parallel

Stage 8:
    * Write orchestration for Stages 1 - 6
    * Stages 1-3 in single TaskGroup called "Ingestion", executed in parallel
    * Stages 4-6 in another TaskGroup called "Processing", executed in parallel
    * 2nd task group should start only after all tasks from the first one are finished
    * After 2nd task group should be a PythonOperator task which would check and print count of the tables in postgres
