using System.Text.Json.Serialization;

namespace SearchDemonSlayerAPI.Models;

public record Character(
    int Id,
    string Name,
    int? Age,
    string Race,
    string Quote,
    [property: JsonPropertyName("img")] string Image);
