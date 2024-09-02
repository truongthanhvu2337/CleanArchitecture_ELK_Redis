**Disclaimer**: This demo may have flaws as it is used for learning purposes. Feel free to correct me if I'm wrong; I would really appreciate that. You can DM me via email at truongthanhvu2337@gmail.com.

## Introduction

This repository is a demo of Clean Architecture with the CQRS pattern, Redis, and the ELK stack (Elasticsearch/Logstash/Kibana). In the future, Elasticsearch may be implemented with multiple nodes, but for now, a single node will be used to keep it simple.

## Installation and setup

First thing you should do is clone the repository

```bash
git clone https://github.com/your/your-project.git

cd your-project/
```
### Setup ELK

This demo will use docker compose to setup ELK stack
```yaml
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
Because this demo uses Logstash with SQL Server,we need to download the driver for SQL Server:
https://learn.microsoft.com/en-us/sql/connect/jdbc/download-microsoft-jdbc-driver-for-sql-server?view=sql-server-ver16

Then put in logstash file like this:
```bash
├── docker-compose.yml
├── logstash
│   ├── config
│   │    └-- logstash.config
│   ├── Dockerfile
│   └── < your driver should be here >
└ 
```
Your can configure the Dockerfile to meet your needs.

```bash
FROM docker.elastic.co/logstash/logstash:8.15.0

RUN rm -f /usr/share/logstash/pipeline/logstash.conf

USER root 

COPY <your driver> /usr/share/logstash/logstash-core/lib/jars/<your driver>
```

And logstash.config

```
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

We're done configuring Logstash. Connecting to Elasticsearch and Kibana is relatively simple because Kibana is already connected in Docker Compose. However, we might need to configure Elasticsearch a bit more. Here, we use the **Elastic.Clients.Elasticsearch** client for .NET.
```cs
var settings = new ElasticsearchClientSettings(new Uri(<you url>));
var client = new ElasticsearchClient(settings);

services.AddSingleton(client);
```
### Setup redis
To setup redis you might download **Microsoft.Extensions.Caching.StackExchangeRedis** package in .NET
then add in your program.cs or DependencyInjection.cs
```csharp
services.AddStackExchangeRedisCache(options =>
{
    var redisConnection = configuration["Redis:HostName"];
    options.Configuration = redisConnection;
    //you can config passsword, user,... if necessary
    
});
```

Config appsetting for redis

```json
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