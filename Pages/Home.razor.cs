using System.Text.Json;
using Microsoft.AspNetCore.Components;
using SearchDemonSlayerAPI.Models;
using SearchDemonSlayerAPI.Services;

namespace SearchDemonSlayerAPI.Pages;

public partial class Home
{
    private const int FeaturedCharacterCount = 5;

    private IReadOnlyList<Character> characters = [];
    private IReadOnlyList<Character> featuredCharacters = [];
    private int? selectedCharacterId;
    private bool isLoading = true;
    private string? errorMessage;

    [Inject]
    private ICharacterService CharacterService { get; set; } = default!;

    private Character? SelectedCharacter =>
        characters.FirstOrDefault(character => character.Id == selectedCharacterId);

    protected override async Task OnInitializedAsync()
    {
        try
        {
            characters = await CharacterService.GetAllAsync();
            selectedCharacterId = characters.FirstOrDefault()?.Id;
            featuredCharacters = characters
                .Where(character => character.Id != selectedCharacterId)
                .OrderBy(_ => Random.Shared.Next())
                .Take(FeaturedCharacterCount)
                .ToList();
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or InvalidOperationException)
        {
            errorMessage = "Não foi possível carregar os personagens. Tente novamente mais tarde.";
        }
        finally
        {
            isLoading = false;
        }
    }

    private void SelectCharacter(int id) => selectedCharacterId = id;
}
