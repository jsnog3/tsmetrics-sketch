-- Seed abtest_metrics table with data for the past 30 days every 2 seconds
-- for test_name feature_a and feature_b, and metric_name response_time, cpu_usage, ram_usage

INSERT INTO abtest_metrics (time, test_name, metric_name, metric_value, unit)
SELECT
    time_point,
    CASE WHEN rn % 2 = 1 THEN 'feature_a' ELSE 'feature_b' END AS test_name,
    metric_name,
    metric_value,
    unit
FROM (
         WITH times AS (
             SELECT
                 generate_series(
                         NOW() - INTERVAL '30 days',
                         NOW(),
                         INTERVAL '2 seconds'
                 ) AS time_point
         ),
              numbered AS (
                  SELECT
                      time_point,
                      ROW_NUMBER() OVER () AS rn
                  FROM times
              ),
              metrics AS (
                  SELECT 'response_time' AS metric_name, 'ms' AS unit
                  UNION ALL
                  SELECT 'cpu_usage', '%'
                  UNION ALL
                  SELECT 'ram_usage', 'MB'
              )
         SELECT
             n.time_point,
             n.rn,
             m.metric_name,
             CASE
                 WHEN m.metric_name = 'response_time' THEN random() * 120 + 30
                 WHEN m.metric_name = 'cpu_usage' THEN random() * 100
                 WHEN m.metric_name = 'ram_usage' THEN random() * 16000 + 1000
                 END AS metric_value,
             m.unit
         FROM numbered n
                  CROSS JOIN metrics m
     ) AS seed_data
WHERE NOT EXISTS (SELECT 1 FROM abtest_metrics);

-- Immediately refresh continuous aggregates so that after seeding the
-- materialized views are populated and queryable without waiting for the
-- scheduled policies to run.
CALL refresh_continuous_aggregate(
    'abtest_metrics_5m',
    start => NOW() - INTERVAL '30 days',
    finish => NOW()
);

CALL refresh_continuous_aggregate(
    'abtest_metrics_30m',
    start => NOW() - INTERVAL '30 days',
    finish => NOW()
);

CALL refresh_continuous_aggregate(
    'abtest_metrics_6h',
    start => NOW() - INTERVAL '30 days',
    finish => NOW()
);
