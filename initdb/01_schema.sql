CREATE EXTENSION IF NOT EXISTS timescaledb;

-- This creates the hypertable in TimescaleDB
-- More information in the documentation:
-- https://docs.tigerdata.com/use-timescale/latest/hypertables/
-- The big advantage is that these hypertables are partitioned by time
-- and these partitions are automatically managed by TimescaleDB
CREATE TABLE IF NOT EXISTS abtest_metrics (
    time TIMESTAMPTZ NOT NULL,
    test_name VARCHAR(50) NOT NULL,
    metric_name VARCHAR(50) NOT NULL,
    metric_value DOUBLE PRECISION NULL,
    unit VARCHAR(20) NOT NULL
) WITH (
    tsdb.hypertable,
    tsdb.partition_column = 'time',
    tsdb.segmentby = 'test_name, metric_name',
    tsdb.orderby = 'time DESC'
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_test_metric_time
    ON abtest_metrics(test_name, metric_name, time);

-- The compression policy will compress our segments
SELECT add_compression_policy('abtest_metrics', INTERVAL '7 days', if_not_exists => TRUE);

-- Drop the chunks in the hypertable that are older than 30 days
-- The aggregation of this data will still be visible in our continuous aggregates
SELECT add_retention_policy('abtest_metrics', INTERVAL '30 days', if_not_exists => TRUE);

-- We could also create a ABTest table where we control for example,
-- if the ABTest is active or not
-- and use as FK in our timeseries database
