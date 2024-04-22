using System.Text.Json.Serialization;
using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Api;

public record class TodoDTO : ITodo
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("is_complete")]
    public bool IsComplete { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; init; }
}
