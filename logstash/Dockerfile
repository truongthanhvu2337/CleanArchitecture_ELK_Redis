FROM docker.elastic.co/logstash/logstash:8.15.0

RUN rm -f /usr/share/logstash/pipeline/logstash.conf

USER root 

COPY mssql-jdbc-12.8.1.jre11.jar /usr/share/logstash/logstash-core/lib/jars/mssql-jdbc-12.8.1.jre11.jar