using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.MigrationService;
using PatriotIndex.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<PatriotIndexDbContext>("PostgresDb", configureDbContextOptions: options =>
{
    options.UseSnakeCaseNamingConvention();
    options.EnableDetailedErrors();
});

builder.Services.AddHostedService<Worker>();

await builder.Build().RunAsync();
