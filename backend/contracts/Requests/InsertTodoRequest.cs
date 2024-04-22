using System.Text.Json.Serialization;
using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Contracts.Requests;

public record InsertTodoRequest
{
    [JsonConstructor]
    public InsertTodoRequest(Todo todo)
    {
        Todo = todo;
    }

    public Todo Todo { get; init; }
}

public record InsertTodoResponse
{
    public InsertTodoResponse() { }

    public InsertTodoResponse(Todo todo)
    {
        Todo = todo;
    }

    public InsertTodoResponse(string error)
    {
        Error = error;
        Todo = null!;
    }

    public Todo? Todo { get; init; }
    public string? Error { get; init; }
}
