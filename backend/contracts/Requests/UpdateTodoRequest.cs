using System.Text.Json.Serialization;
using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Contracts.Requests;

public record UpdateTodoRequest
{
    [JsonConstructor]
    public UpdateTodoRequest(Todo todo)
    {
        Todo = todo;
    }

    public Todo Todo { get; init; }
}

public record UpdateTodoResponse
{
    public UpdateTodoResponse() { }

    public UpdateTodoResponse(Todo todo)
    {
        Todo = todo;
    }

    public UpdateTodoResponse(string error)
    {
        Error = error;
        Todo = null!;
    }

    public Todo? Todo { get; init; }
    public string? Error { get; init; }
}
