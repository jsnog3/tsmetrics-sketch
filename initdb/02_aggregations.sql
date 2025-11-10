-- Other feature of timescaledb is the continuous aggregate to precompute and aggregate data
-- https://docs.tigerdata.com/use-timescale/latest/continuous-aggregates/about-continuous-aggregates/
CREATE MATERIALIZED VIEW abtest_metrics_5m
WITH (timescaledb.continuous) AS
SELECT 
    test_name,
    time_bucket(INTERVAL '5 min', time) AS bucket_5m,
    AVG(metric_value),
    MIN(metric_value),
    MAX(metric_value)
FROM abtest_metrics
GROUP BY test_name, bucket_5m;

SELECT add_continuous_aggregate_policy(
    'abtest_metrics_5m',
    start_offset => INTERVAL '3 days',
    end_offset => INTERVAL '5 min',
    schedule_interval => INTERVAL '5 min',
    if_not_exists => TRUE);

-- We can build other continuous aggregate on top of existing ones
-- saving some computation already done
CREATE MATERIALIZED VIEW abtest_metrics_30m
WITH (timescaledb.continuous) AS
SELECT
    test_name,
    time_bucket(INTERVAL '30 min', bucket_5m) AS bucket_30m,
    AVG(avg),
    MIN(min),
    MAX(max)
FROM abtest_metrics_5m
GROUP BY test_name, bucket_30m;

SELECT add_continuous_aggregate_policy(
    'abtest_metrics_30m',
    start_offset => INTERVAL '3 days',
    end_offset => INTERVAL '30 min',
    schedule_interval => INTERVAL '30 min',
    if_not_exists => TRUE);

CREATE MATERIALIZED VIEW abtest_metrics_6h
WITH (timescaledb.continuous) AS
SELECT
    test_name,
    time_bucket(INTERVAL '6 hours', bucket_30m) AS bucket_6h,
    AVG(avg),
    MIN(min),
    MAX(max)
FROM abtest_metrics_30m
GROUP BY test_name, bucket_6h;

SELECT add_continuous_aggregate_policy(
    'abtest_metrics_6h',
    start_offset => INTERVAL '3 days',
    end_offset => INTERVAL '6 hours',
    schedule_interval => INTERVAL '6 hours',
    if_not_exists => TRUE);
