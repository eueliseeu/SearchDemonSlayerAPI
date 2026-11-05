using System.Net.Http.Json;
using SearchDemonSlayerAPI.Models;

namespace SearchDemonSlayerAPI.Services;

public class CharacterService(HttpClient httpClient) : ICharacterService
{
    private IReadOnlyList<Character>? cache;

    public async Task<IReadOnlyList<Character>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (cache is not null)
        {
            return cache;
        }

        var firstPage = await GetPageAsync(1, cancellationToken);
        var remainingPages = await Task.WhenAll(
            Enumerable
                .Range(2, Math.Max(0, firstPage.Pagination.TotalPages - 1))
                .Select(page => GetPageAsync(page, cancellationToken)));

        cache = firstPage.Content
            .Concat(remainingPages.SelectMany(response => response.Content))
            .OrderBy(character => character.Id)
            .ToList();

        return cache;
    }

    private async Task<CharactersResponse> GetPageAsync(int page, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<CharactersResponse>($"characters?page={page}", cancellationToken);
        return response ?? throw new InvalidOperationException("A API retornou uma resposta vazia.");
    }
}
