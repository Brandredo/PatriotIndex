using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("Postgres", port: 5432)
    .WithDataVolume("patriotindex-postgres-data", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("PostgresDb");

builder.AddProject<PatriotIndex_Domain>("patriotindex-domain");

var api = builder.AddProject<PatriotIndex_Scheduler>("patriotindex-scheduler")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.Build().Run();