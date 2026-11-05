namespace SearchDemonSlayerAPI.Models;

public record CharactersResponse(Pagination Pagination, IReadOnlyList<Character> Content);
