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
                  SELECT
                      'response_time' AS metric_name, random() * 120 + 30 AS metric_value, 'ms' AS unit
                  UNION ALL
                  SELECT
                      'cpu_usage', random() * 100, '%'
                  UNION ALL
                  SELECT
                      'ram_usage', random() * 16000 + 1000, 'MB'
              )
         SELECT
             n.time_point,
             n.rn,
             m.metric_name,
             m.metric_value,
             m.unit
         FROM numbered n
                  CROSS JOIN metrics m
     ) AS seed_data
WHERE NOT EXISTS (SELECT 1 FROM abtest_metrics);
