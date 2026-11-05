using SearchDemonSlayerAPI.Models;

namespace SearchDemonSlayerAPI.Services;

public interface ICharacterService
{
    Task<IReadOnlyList<Character>> GetAllAsync(CancellationToken cancellationToken = default);
}
