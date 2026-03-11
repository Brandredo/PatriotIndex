insert into conferences(id, name, alias) values ('1bdefe12-6cb2-4d6a-b208-b04602ae79c3', 'AFC', 'AFC');
insert into conferences(id, name, alias) values ('b1808e5f-d40b-47c0-8af8-5175c0fdcd26', 'NFC', 'NFC');

insert into divisions(id, name, alias, conference_id) values ('324decdd-aa1b-4074-8958-c009d8fac31a', 'AFC West', 'AFC_WEST', '1bdefe12-6cb2-4d6a-b208-b04602ae79c3');
insert into divisions(id, name, alias, conference_id) values ('b95cd27d-d631-4fe1-bc05-0ae47fc0b14b', 'AFC East', 'AFC_EAST', '1bdefe12-6cb2-4d6a-b208-b04602ae79c3');
insert into divisions(id, name, alias, conference_id) values ('e447e7c0-5997-4bb7-bea3-aaae48aedcb8', 'AFS South', 'AFS_SOUTH', '1bdefe12-6cb2-4d6a-b208-b04602ae79c3');
insert into divisions(id, name, alias, conference_id) values ('eb60da78-4eb5-4184-971e-2c5cd4ab4988', 'AFC North', 'AFC_NORTH', '1bdefe12-6cb2-4d6a-b208-b04602ae79c3');


insert into divisions(id, name, alias, conference_id) values ('2a46bf95-b036-4c6c-b69f-a80c4b6c46bf', 'NFC South', 'NFC_SOUTH', 'b1808e5f-d40b-47c0-8af8-5175c0fdcd26');
insert into divisions(id, name, alias, conference_id) values ('390d000d-9949-42e6-bf5e-b166dc463675', 'NFC West', 'NFC_WEST', 'b1808e5f-d40b-47c0-8af8-5175c0fdcd26');
insert into divisions(id, name, alias, conference_id) values ('6dab3ca1-b9cb-403f-91dd-b2a3708ab060', 'NFC East', 'NFC_EAST', 'b1808e5f-d40b-47c0-8af8-5175c0fdcd26');
insert into divisions(id, name, alias, conference_id) values ('6dc1933f-ca90-46e6-aaf7-9b95cc44306a', 'NFC North', 'NFC_NORTH', 'b1808e5f-d40b-47c0-8af8-5175c0fdcd26');

commit;

select name, count(*) from players group by name having count(*) > 1;

select team_id, count(*) from players group by team_id having count(*) > 0;

select * from players where name = 'Jaylon Jones';
commit;

select A.id, count(1) from divisions A join teams B on A.id = B.division_id group by A.id having count(1) >= 1;

select * from sync_logs;
select * from conferences;
select * from divisions;
select * from teams;
select * from team_colors;
select * from players;
select * from games;
select * from periods;
select * from drives;
select * from drives order by sequence;

-- truncate table games cascade;
-- truncate table periods;
-- truncate table coin_tosses cascade;
-- truncate table drives;
-- truncate table pbp_drive_events;
-- truncate table pbp_event_statistics;
-- 
-- 
-- truncate table players;
-- truncate table teams;

CREATE SCHEMA IF NOT EXISTS hangfire;

-- Allow the user to use the schema
GRANT USAGE ON SCHEMA hangfire TO postgres;

-- Allow the user to create tables (needed on first run)
GRANT CREATE ON SCHEMA hangfire TO postgres;

-- Allow full access to all current tables in the schema
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA hangfire TO postgres;

-- Allow full access to any tables created in the future
ALTER DEFAULT PRIVILEGES IN SCHEMA hangfire
    GRANT ALL PRIVILEGES ON TABLES TO postgres;

-- Same for sequences (Hangfire uses these for IDs)
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA hangfire TO postgres;

ALTER DEFAULT PRIVILEGES IN SCHEMA hangfire
    GRANT ALL PRIVILEGES ON SEQUENCES TO postgres;



select * from hangfire.jobqueue;
select * from hangfire.job order by createdat desc;
--delete from hangfire.jobqueue where queue = 'default';

commit;


update teams set is_active=true where id != '9dbb9060-ba0f-4920-829e-16d4d9246b5d';


INSERT INTO teams (id, name, market, alias) VALUES
                                                ('f0e724b0-4cbf-495a-be47-013907608da9', '49ers', 'San Francisco', 'SF'),
                                                ('7b112545-38e6-483c-a55c-96cf6ee49cb8', 'Bears', 'Chicago', 'CHI'),
                                                ('ad4ae08f-d808-42d5-a1e6-e9bc4e34d123', 'Bengals', 'Cincinnati', 'CIN'),
                                                ('768c92aa-75ff-4a43-bcc0-f2798c2e1724', 'Bills', 'Buffalo', 'BUF'),
                                                ('ce92bd47-93d5-4fe9-ada4-0fc681e6caa0', 'Broncos', 'Denver', 'DEN'),
                                                ('d5a2eb42-8065-4174-ab79-0a6fa820e35e', 'Browns', 'Cleveland', 'CLE'),
                                                ('4254d319-1bc7-4f81-b4ab-b5e6f3402b69', 'Buccaneers', 'Tampa Bay', 'TB'),
                                                ('de760528-1dc0-416a-a978-b510d20692ff', 'Cardinals', 'Arizona', 'ARI'),
                                                ('1f6dcffb-9823-43cd-9ff4-e7a8466749b5', 'Chargers', 'Los Angeles', 'LAC'),
                                                ('6680d28d-d4d2-49f6-aace-5292d3ec02c2', 'Chiefs', 'Kansas City', 'KC'),
                                                ('82cf9565-6eb9-4f01-bdbd-5aa0d472fcd9', 'Colts', 'Indianapolis', 'IND'),
                                                ('22052ff7-c065-42ee-bc8f-c4691c50e624', 'Commanders', 'Washington', 'WAS'),
                                                ('e627eec7-bbae-4fa4-8e73-8e1d6bc5c060', 'Cowboys', 'Dallas', 'DAL'),
                                                ('4809ecb0-abd3-451d-9c4a-92a90b83ca06', 'Dolphins', 'Miami', 'MIA'),
                                                ('386bdbf9-9eea-4869-bb9a-274b0bc66e80', 'Eagles', 'Philadelphia', 'PHI'),
                                                ('e6aa13a4-0055-48a9-bc41-be28dc106929', 'Falcons', 'Atlanta', 'ATL'),
                                                ('04aa1c9d-66da-489d-b16a-1dee3f2eec4d', 'Giants', 'New York', 'NYG'),
                                                ('f7ddd7fa-0bae-4f90-bc8e-669e4d6cf2de', 'Jaguars', 'Jacksonville', 'JAC'),
                                                ('5fee86ae-74ab-4bdd-8416-42a9dd9964f3', 'Jets', 'New York', 'NYJ'),
                                                ('c5a59daa-53a7-4de0-851f-fb12be893e9e', 'Lions', 'Detroit', 'DET'),
                                                ('a20471b4-a8d9-40c7-95ad-90cc30e46932', 'Packers', 'Green Bay', 'GB'),
                                                ('f14bf5cc-9a82-4a38-bc15-d39f75ed5314', 'Panthers', 'Carolina', 'CAR'),
                                                ('97354895-8c77-4fd4-a860-32e62ea7382a', 'Patriots', 'New England', 'NE'),
                                                ('7d4fcc64-9cb5-4d1b-8e75-8a906d1e1576', 'Raiders', 'Las Vegas', 'LV'),
                                                ('2eff2a03-54d4-46ba-890e-2bc3925548f3', 'Rams', 'Los Angeles', 'LA'),
                                                ('ebd87119-b331-4469-9ea6-d51fe3ce2f1c', 'Ravens', 'Baltimore', 'BAL'),
                                                ('0d855753-ea21-4953-89f9-0e20aff9eb73', 'Saints', 'New Orleans', 'NO'),
                                                ('3d08af9e-c767-4f88-a7dc-b920c6d2b4a8', 'Seahawks', 'Seattle', 'SEA'),
                                                ('cb2f9f1f-ac67-424e-9e72-1475cb0ed398', 'Steelers', 'Pittsburgh', 'PIT'),
                                                ('82d2d380-3834-4938-835f-aec541e5ece7', 'Texans', 'Houston', 'HOU'),
                                                ('d26a1ca5-722d-4274-8f97-c92e49c96315', 'Titans', 'Tennessee', 'TEN'),
                                                ('33405046-04ee-4058-a950-d606f8c30852', 'Vikings', 'Minnesota', 'MIN');




INSERT INTO teams (id, name, market, alias) VALUES
    ('9dbb9060-ba0f-4920-829e-16d4d9246b5d', 'Chargers', 'San Diego', 'SD');


update teams set is_active=true where id != '9dbb9060-ba0f-4920-829e-16d4d9246b5d';
update conferences set name='NFC' where id='b1808e5f-d40b-47c0-8af8-5175c0fdcd26';

