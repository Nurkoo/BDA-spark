FROM bitnami/spark:3

USER root

RUN apt-get update && apt-get install -y openssh-server sudo

RUN useradd -u 1001 -g 0 -m spark_user

RUN echo spark_user:Spark123@ | chpasswd

RUN usermod -a -G sudo spark_user

RUN mkdir -p /opt/bitnami/spark/dev
RUN mkdir /opt/bitnami/spark/dev/scripts /opt/bitnami/spark/dev/jars

COPY --chown=spark_user:root docker_conf/postgresql-42.5.4.jar /opt/bitnami/spark/dev/jars

USER spark_user