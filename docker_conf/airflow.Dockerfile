FROM apache/airflow:2.5.0-python3.8 AS airflow

USER airflow

COPY docker_conf/airflow_requirements.txt .
RUN python3 -m pip install -r airflow_requirements.txt

USER root
ADD ./docker_conf/postgresql-42.5.4.jar /opt/airflow/3rdParty/jars/postgresql-42.5.4.jar

RUN chmod 777 /opt/airflow/3rdParty/jars/postgresql-42.5.4.jar

USER airflow