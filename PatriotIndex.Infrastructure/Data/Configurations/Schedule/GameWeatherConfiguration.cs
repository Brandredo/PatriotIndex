using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Infrastructure.Data.Configurations.Schedule;

public class GameWeatherConfiguration : IEntityTypeConfiguration<GameWeather>
{
    public void Configure(EntityTypeBuilder<GameWeather> entity)
    {
        entity.ToTable("game_weather");
        entity.HasKey(e => e.GameId);
        entity.Property(e => e.Condition).HasMaxLength(50);
        entity.Property(e => e.WindDirection).HasMaxLength(10);
        entity.HasOne(e => e.Game)
            .WithOne(g => g.Weather)
            .HasForeignKey<GameWeather>(e => e.GameId);
    }
}
