using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// var apiKey = builder.AddParameter("");
// var apiBaseUrl = builder.AddParameter("");

var postgresdb = builder.AddConnectionString("PostgresDb");
var cache = builder.AddConnectionString("Cache");

var migrations = builder.AddProject<PatriotIndex_MigrationService>("patriotindex-migrations")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddProject<PatriotIndex_Scheduler>("patriotindex-scheduler")
    .WithReference(postgresdb)
    .WaitForCompletion(migrations);

builder.AddProject<PatriotIndex_Api>("patriotindex-api")
    .WithReference(postgresdb)
    .WithReference(cache)
    .WaitForCompletion(migrations);

builder.Build().Run();