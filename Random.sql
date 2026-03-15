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
select * from seasons;
select * from team_season_stats;
select * from player_season_stats;


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