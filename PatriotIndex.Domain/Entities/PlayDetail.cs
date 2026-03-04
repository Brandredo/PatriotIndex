using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Domain.Entities;

[Owned]
public class PlayDetailPlayer
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(5)]
    public string? Position { get; set; }
}

[Owned]
public class PlayDetail
{
    public long Sequence { get; set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public List<PlayDetailPlayer> Players { get; set; } = new();
}
