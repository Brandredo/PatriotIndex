using Microsoft.Extensions.Configuration;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var pgPassword = builder.AddParameter("Postgres-password", secret: true);
int.TryParse(builder.Configuration["Parameters:Postgres-port"], out var pgPort);

var postgres = builder.AddPostgres("Postgres", password: pgPassword)
    .WithEndpoint(name: "postgresendpoint", scheme: "tcp", port: pgPort, targetPort: 5432, isProxied: false)
    .WithDataVolume("patriotindex-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName("patriotindex-postgres");

var postgresdb = postgres.AddDatabase("PostgresDb");

builder.AddProject<PatriotIndex_Scheduler>("patriotindex-scheduler")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddProject<PatriotIndex_Api>("patriotindex-api")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.Build().Run();