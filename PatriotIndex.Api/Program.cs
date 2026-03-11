using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Interfaces;
using PatriotIndex.Domain.Queries;
using PatriotIndex.ServiceDefaults;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<PatriotIndexDbContext>("PostgresDb", configureDbContextOptions: options =>
{
    //var connectionString = builder.Configuration.GetConnectionString("PatriotIndexDb");
    options.UseSnakeCaseNamingConvention();
    options.EnableDetailedErrors();
});

// Read repositories
builder.Services.AddScoped<ITeamRepository, TeamQueryRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerQueryRepository>();
builder.Services.AddScoped<IGameRepository, GameQueryRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardQueryRepository>();
builder.Services.AddScoped<IConferenceRepository, ConferenceQueryRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseHttpsRedirection();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

await app.RunAsync();
