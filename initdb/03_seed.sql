-- Seed metrics table with data for the past day every 2 seconds
-- evenly split between feature_a and feature_b
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
    )
INSERT INTO metrics (time, experiment, response)
SELECT
    time_point,
    CASE WHEN rn % 2 = 1 THEN 'feature_a' ELSE 'feature_b' END AS experiment,
    random() * 120 + 30 -- random response between 30 and 150
FROM numbered;

