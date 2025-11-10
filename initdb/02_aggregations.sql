-- Other feature of timescaledb is the continuous aggregate to precompute and aggregate data
-- https://docs.tigerdata.com/use-timescale/latest/continuous-aggregates/about-continuous-aggregates/
CREATE MATERIALIZED VIEW metrics_5m
WITH (timescaledb.continuous) AS
SELECT 
    experiment,
    time_bucket(INTERVAL '5 min', time) AS bucket_5m,
    AVG(response)
FROM metrics
GROUP BY experiment, bucket_5m;

SELECT add_continuous_aggregate_policy(
    'metrics_5m',
    start_offset => INTERVAL '7 days',
    end_offset => INTERVAL '5 min',
    schedule_interval => INTERVAL '5 min',
    if_not_exists => TRUE);

-- We can build other continuous aggregate on top of existing ones
-- saving some computation already done
CREATE MATERIALIZED VIEW metrics_30m
WITH (timescaledb.continuous) AS
SELECT
    experiment,
    time_bucket(INTERVAL '30 min', bucket_5m) AS bucket_30m,
    AVG(avg)
FROM metrics_5m
GROUP BY experiment, bucket_30m;

SELECT add_continuous_aggregate_policy(
    'metrics_30m',
    start_offset => INTERVAL '7 days',
    end_offset => INTERVAL '30 min',
    schedule_interval => INTERVAL '30 min',
    if_not_exists => TRUE);