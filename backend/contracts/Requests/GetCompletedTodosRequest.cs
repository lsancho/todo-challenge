using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Contracts.Requests;

public record GetCompletedTodosRequest
{
    public GetCompletedTodosRequest() { }
}

public record GetCompletedTodosResponse
{
    public GetCompletedTodosResponse(List<Todo> todos)
    {
        Todos = todos;
    }

    public GetCompletedTodosResponse()
    {
        Todos = new List<Todo>();
    }

    public List<Todo> Todos { get; init; }
}
