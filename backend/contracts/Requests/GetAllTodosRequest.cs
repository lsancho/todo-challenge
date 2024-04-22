using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Contracts.Requests;

public record GetAllTodosRequest
{
    public GetAllTodosRequest() { }
}

public record GetAllTodosResponse
{
    public GetAllTodosResponse(List<Todo> todos)
    {
        Todos = todos;
    }

    public GetAllTodosResponse()
    {
        Todos = new List<Todo>();
    }

    public List<Todo> Todos { get; init; }
}
