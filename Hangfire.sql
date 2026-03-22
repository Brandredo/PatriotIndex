CREATE SCHEMA IF NOT EXISTS hangfire;

-- Allow the user to use the schema
GRANT USAGE ON SCHEMA hangfire TO patriotindex;

-- Allow the user to create tables (needed on first run)
GRANT CREATE ON SCHEMA hangfire TO patriotindex;

-- Allow full access to all current tables in the schema
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA hangfire TO patriotindex;

-- Allow full access to any tables created in the future
ALTER DEFAULT PRIVILEGES IN SCHEMA hangfire
    GRANT ALL PRIVILEGES ON TABLES TO patriotindex;

-- Same for sequences (Hangfire uses these for IDs)
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA hangfire TO patriotindex;

ALTER DEFAULT PRIVILEGES IN SCHEMA hangfire
    GRANT ALL PRIVILEGES ON SEQUENCES TO patriotindex;



select * from hangfire.jobqueue;
select * from hangfire.job order by createdat desc;
--delete from hangfire.jobqueue where queue = 'default';

commit;
