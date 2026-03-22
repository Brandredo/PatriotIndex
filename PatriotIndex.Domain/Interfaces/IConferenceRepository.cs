using PatriotIndex.Domain.DTOs;

namespace PatriotIndex.Domain.Interfaces;

public interface IConferenceRepository
{
    Task<IReadOnlyList<ConferenceWithDivisionsDto>> GetAllWithDivisionsAsync();
    Task<IReadOnlyList<ConferenceWithDivisionsDto>> GetAllConferencesAsync();
}
