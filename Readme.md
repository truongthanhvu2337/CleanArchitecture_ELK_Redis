Disclaimer: this demo may have flaws because it use for learning purpose. Feel free to correct me if i'm wrong, really appreciate that. you can dm me via email: truongthanhvu2337@gmail.com 

## Introduction

This repository is a demo of clean architecture with CQRS pattern,Redis and ELK (Elasticsearch/Logstash/Kibana) stack. In the future may implement Elasticsearch with mutiple node but at the time will use single-node to keep it simple

## Installation and setup

First thing you should do is clone the repository

```shell
git clone https://github.com/your/your-project.git

cd your-project/
```
### Setup ELK

This demo will use docker compose to setup ELK stack
```shell
services:
  #orther services...

  elasticsearch:
    image: elasticsearch:8.15.0
    container_name: elasticsearch
    environment:
      - discovery.type=single-node
      - xpack.security.enabled=false
    volumes:
      - elasticsearch-data:/usr/share/elasticsearch/data
    ports:
      - "9200:9200"

  logstash:
    build: ./logstash
    environment:
      LS_JAVA_OPTS: "-Xmx256m -Xms256m"
    ports:
      - 5001:5001
    container_name: logstash
    # networks:
    #   - elastic
    depends_on:
      - elasticsearch
    volumes:
      - ./logstash/config:/usr/share/logstash/pipeline

  kibana:
      container_name: kibana
      image: kibana:8.15.0
      ports:
      - "5601:5601"
      depends_on:
      - elasticsearch
      environment:
      - ELASTICSEARCH_URL=http://elasticsearch:9200
      
  #other services...
      
volumes:
  elasticsearch-data:
```
Because this demo using logstash with sqlserver so we have to download the driver for sql server:
https://learn.microsoft.com/en-us/sql/connect/jdbc/download-microsoft-jdbc-driver-for-sql-server?view=sql-server-ver16

Then put in logstash file like this:
```shell
├── docker-compose.yml
├── logstash
│   ├── config
│   │    └-- logstash.config
│   ├── Dockerfile
│   └── < your driver should be here >
└ 
```
Your can config Dockerfile and to meet your need

```shell
FROM docker.elastic.co/logstash/logstash:8.15.0

RUN rm -f /usr/share/logstash/pipeline/logstash.conf

USER root 

COPY <your driver> /usr/share/logstash/logstash-core/lib/jars/<your driver>
```

and logstash.config

```shell
input {
  jdbc {
    jdbc_connection_string => "jdbc:sqlserver://<your host>;databaseName=<your database>;user=<your username>;password=<your password>;encrypt=false"
    jdbc_user => "<your user name>"
    jdbc_password => "<your password>"
    jdbc_driver_class => "com.microsoft.sqlserver.jdbc.SQLServerDriver"
	jdbc_driver_library => "/usr/share/logstash/logstash-core/lib/jars/<your driver"
    statement => "SELECT id, name, address  FROM [dbo].[customers]"
	
	# minute - hour - day - month - weekday for each cron
    schedule => "*/10 * * * *"
	
	tracking_column => "id"
	# use_column_value => true
	# tracking_column_type => "numeric"
	clean_run => false
  }
}

filter {
	mutate {
	  remove_field => ["@timestamp", "@version"]
	}
}
output {
  elasticsearch {
    hosts => ["http://elasticsearch:9200"]
    index => "customer"
	document_id => "%{id}"
  }
  
  stdout { 
    # codec => json_lines 
	codec => rubydebug
  }
}
```

We done for config logstash, as for connection to elasticsearch and kibana kinda simple cause kibana already connect in docker compose but elastic search me may config a little. 
Here we use Elastic.Client.Elasticsearch client for .Net
```shell
var settings = new ElasticsearchClientSettings(new Uri(<you url>));
var client = new ElasticsearchClient(settings);

services.AddSingleton(client);
```
### Setup redis
To setup redis you should download Microsoft.Extensions.Caching.StackExchangeRedis package in .NET
then add in your program.cs or DependencyInjection.cs
```shell
services.AddStackExchangeRedisCache(options =>
{
    var redisConnection = configuration["Redis:HostName"];
    options.Configuration = redisConnection;
    //you can config passsword, user,... if necessary
    
});
```

Config appsetting for redis

```shell
"Redis": {
  "HostName": "redis:6379",
},
```
## Contributing

If you'd like to contribute to Project Title, here are some guidelines:

1. Fork the repository.
2. Create a new branch for your changes.
3. Make your changes.
4. Write tests to cover your changes.
5. Run the tests to ensure they pass.
6. Commit your changes.
7. Push your changes to your forked repository.
8. Submit a pull request.

## Authors and Acknowledgment

This demo was created by **[truongthanhvu2337](https://github.com/truongthanhvu2337)**.