-- Other feature of timescaledb is the continuous aggregate to precompute and aggregate data
-- https://docs.tigerdata.com/use-timescale/latest/continuous-aggregates/about-continuous-aggregates/
CREATE MATERIALIZED VIEW abtest_metrics_5m
WITH (timescaledb.continuous) AS
SELECT
    test_name,
    metric_name,
    unit,
    time_bucket(INTERVAL '5 minutes', time) AS bucket_start,
    AVG(metric_value) AS avg_value,
    MIN(metric_value) AS min_value,
    MAX(metric_value) AS max_value
FROM abtest_metrics
GROUP BY test_name, metric_name, unit, time_bucket(INTERVAL '5 minutes', time);

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
    metric_name,
    unit,
    time_bucket(INTERVAL '30 minutes', bucket_start) AS bucket_start,
    AVG(avg_value) AS avg_value,
    MIN(min_value) AS min_value,
    MAX(max_value) AS max_value
FROM abtest_metrics_5m
GROUP BY
    test_name,
    metric_name,
    unit,
    time_bucket(INTERVAL '30 minutes', bucket_start);

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
    metric_name,
    unit,
    time_bucket(INTERVAL '6 hours', bucket_start) AS bucket_start,
    AVG(avg_value) AS avg_value,
    MIN(min_value) AS min_value,
    MAX(max_value) AS max_value
FROM abtest_metrics_30m
GROUP BY
    test_name,
    metric_name,
    unit,
    time_bucket(INTERVAL '6 hours', bucket_start);

SELECT add_continuous_aggregate_policy(
    'abtest_metrics_6h',
    start_offset => INTERVAL '3 days',
    end_offset => INTERVAL '6 hours',
    schedule_interval => INTERVAL '6 hours',
    if_not_exists => TRUE);
