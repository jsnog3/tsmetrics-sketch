# TSMetrics

#### Overview

The goal of this project is to use time series data containing key metrics (e.g., CPU usage, response times, add-to-cart clicks) from our main service (such as an e-commerce website) to enable simple A/B testing during feature rollouts.

![Architecture Diagram](docs/architecture.png)

1. A public web service (e.g., an e-commerce platform) where we implement A/B testing.
2. The web service publishes messages containing various metrics to a message queue.
3. A consumer service reads these messages and stores the data in a database.
4. A REST API provides endpoints to retrieve and display these metrics from the database.
5. An internal dashboard (e.g., a Streamlit application in Python) visualizes this information for analysis.

The current implementation is focused on point 4.

#### Features

The focus was to create an API that provides a good performance. To achieve this, some decisions were made:


- A dedicated database was used to handle time-series data. TimescaleDB offers scalability, usability, and reliability as an extension of PostgreSQL. It also provides specialized features that simplify the process, such as automatic partitioning (chunking) and continuous aggregations.
- Trim unnecessary data from the payload to make responses smaller.
- Implement rate limiting and set limits on the amount of data retrievable from the API.
- Indicate whether more data is available for a given query without revealing the exact count.
- Restrict the raw TimescaleDB hypertable to write operations only.

#### Local Testing

**Prerequisites:** Docker

```
docker compose up -d
```

This command starts two containers (TimescaleDb and .Net REST API). 
Some data is automatic seeded in database (can take some seconds) so the API should be ready to use.

For swagger UI use this url:
```
http://localhost:5221/swagger
```

To test the api use for example this:

```
http://localhost:5221/metrics?abTestName=feature_a&metricName=response_time&start=2025-10-13T12:00:00Z&end=2025-11-11T12:00:00Z&granularity=5m
```

An example response from the API:

```json
{
  "abTest": "feature_a",
  "metric": "response_time",
  "unit": "ms",
  "skip": 0,
  "limit": 2,
  "count": 2,
  "isComplete": true,
  "datapoints": [
    {
      "t": 1762689900,
      "a": 98.598,
      "l": 33.062,
      "h": 149.858
    },
    {
      "t": 1762690200,
      "a": 88.466,
      "l": 30.614,
      "h": 144.186
    }
  ]
}
```

#### Possible Next Steps

- Implement Authorization and Authentication
- Manage secrets
- Create a dashboard with streamlit in python to show this data in a web interface for visualization and analysis
- Add a cache layer repository
- Improve test coverage (very low right now)
- Implement ABTest endpoint and metrics
- Create new tables to manage abtest and metrics

#### Local Development

**Prerequisites:** Docker, Docker Compose, .Net 9 SDK

```
docker compose up timescaledb -d
docker 
dotnet build
dotnet run --project src/TSMetricsAPI/TSMetricsAPI.csproj
```
