CREATE TYPE experiment_status AS ENUM ('active', 'paused');

CREATE TABLE IF NOT EXISTS ab_experiments (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    start_date DATE NOT NULL,
    end_date DATE,
    status experiment_status NOT NULL
);
