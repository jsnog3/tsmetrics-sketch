CREATE EXTENSION IF NOT EXISTS timescaledb;

-- This creates the hypertable in TimescaleDB
-- More information in the documentation:
-- https://docs.tigerdata.com/use-timescale/latest/hypertables/
-- The big advantage is that these hypertables are partitioned by time
-- and these partitions are automatically managed by TimescaleDB
CREATE TABLE IF NOT EXISTS metrics (
    time TIMESTAMPTZ NOT NULL,
    experiment VARCHAR(50) NOT NULL,
    response DOUBLE PRECISION NULL
) WITH (
    tsdb.hypertable,
    tsdb.partition_column = 'time',
    tsdb.segmentby = 'experiment',
    tsdb.orderby = 'time DESC'
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_experiment_time
    ON metrics(experiment, time);

-- The compression policy will compress our segments
SELECT add_compression_policy('metrics', INTERVAL '7 days', if_not_exists => TRUE);

-- Drop the chunks in the hypertable that are older than 30 days
-- The aggregation of this data will still be visible in our continuous aggregates
SELECT add_retention_policy('metrics', INTERVAL '30 days', if_not_exists => TRUE);
