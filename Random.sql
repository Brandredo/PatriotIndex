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
select * from games order by season_year desc;
select * from periods;
select * from drives;
select * from drives order by sequence;
select * from seasons;
select * from team_season_stats;
select * from player_season_stats;

truncate table players cascade;
truncate table team_season_stats;
truncate table player_season_stats;
truncate table team_game_stats;



-- delete from team_season_stats where season_year = 2026;
-- delete from player_season_stats where season_year = 2026;
-- commit;

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

select * from seasons;


select * from periods where game_id='2ae62361-eb2f-431f-84b1-def2a80f98d5';

-- 
-- UPDATE players SET position = CASE position
--                                   WHEN '0'  THEN 'C'   WHEN '1'  THEN 'CB'  WHEN '2'  THEN 'DB'
--                                   WHEN '3'  THEN 'DE'  WHEN '4'  THEN 'DL'  WHEN '5'  THEN 'DT'
--                                   WHEN '6'  THEN 'FB'  WHEN '7'  THEN 'FS'  WHEN '8'  THEN 'G'
--                                   WHEN '9'  THEN 'LB'  WHEN '10' THEN 'LS'  WHEN '11' THEN 'MLB'
--                                   WHEN '12' THEN 'NT'  WHEN '13' THEN 'OG'  WHEN '14' THEN 'OT'
--                                   WHEN '15' THEN 'OL'  WHEN '16' THEN 'OLB' WHEN '17' THEN 'P'
--                                   WHEN '18' THEN 'QB'  WHEN '19' THEN 'RB'  WHEN '20' THEN 'SAF'
--                                   WHEN '21' THEN 'SS'  WHEN '22' THEN 'T'   WHEN '23' THEN 'TE'
--                                   WHEN '24' THEN 'WR'  WHEN '25' THEN 'K'
--                                   ELSE position
--     END
-- WHERE position ~ '^[0-9]+$';