Tasks:

Stage 0:
    - Prepare the script that creates s3 bucket called "bda-data-bucket" (mocked by localsttack - available at https://localhost:4566)
    - Upload sample .txt file to the bucket and verify that it's working
    - Useful links:
        - https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/s3.html
        - https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/s3/client/create_bucket.html
        - https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/s3/client/put_object.html
    - Localstack credentials:
        - region_name="eu-west-1",
        - aws_access_key_id="fake_key",
        - aws_secret_access_key="fake_key",
        - endpoint="http://localhost:4566" (if accessed from local machine),
        - endpoint="http://localstack:4566" (if accessed from another container ie spark container),
    - View bucket contents from the web browser:
        - http://localhost:4566/<bucket_name>

Stage 1:
    - Change the implementation of API reader for claim-hub
    - write a method that reads the data 5 times
    - Each file should be written in AWS S3 (mocked by localsttack - available at https://localhost:4566)
    - s3://bda-data-bucket/claim-hub/<execution_date>/<table_name>_part(part_number 1 - 5).json

Stage 2:
    - Change the implementation of API reader for insure-wave
    - write a method that reads the data 5 times
    - Each file should be written in AWS S3 (mocked by localsttack - available at https://localhost:4566)
    - s3://bda-data-bucket/insure-wave/<execution_date>/<table_name>_part(part_number 1 - 5).json

Stage 3:
    - Change the implementation of API reader for claim-zone
    - write a method that reads the data 5 times
    - Each file should be written in AWS S3 (mocked by localsttack - available at https://localhost:4566)
    - s3://bda-data-bucket/claim-zone/<execution_date>/<table_name>_part(part_number 1 - 5).json

Stage 4:
    - Create a Spark job that reads all data for a claim table from three sources (AWS S3 paths) for a single day
    - The data should be read from each view from 5 parts saved in previous step
    - The date for which the data should be retrieved should be passed as an argument with spark-submit
    - Combine all parts and all views into one table
    - Remove duplicates
    - Add another column called "exec_date" and fill it with the date which was read from the AWS S3
    - Save final table in postgres

Stage 5:
    - Create a Spark job that reads all data for a motor table from three sources (AWS S3 paths) for a single day
    - The data should be read from each view from 5 parts saved in previous step
    - The date for which the data should be retrieved should be passed as an argument with spark-submit
    - Combine all parts and all views into one table
    - Remove duplicates
    - Add another column called "exec_date" and fill it with the date which was read from the AWS S3
    - Save final table in postgres

Stage 6:
    - Create a Spark job that reads all data for a exposure table from three sources (AWS S3 paths) for a single day
    - The data should be read from each view from 5 parts saved in previous step
    - The date for which the data should be retrieved should be passed as an argument with spark-submit
    - Combine all parts and all views into one table
    - Remove duplicates
    - Add another column called "exec_date" and fill it with the date which was read from the AWS S3
    - Save final table in postgres