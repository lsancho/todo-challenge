using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Contracts.Requests;

public record GetTodoByIdRequest
{
    public GetTodoByIdRequest() { }

    public GetTodoByIdRequest(int id)
    {
        Id = id;
    }

    public int Id { get; init; }
}

public record GetTodoByIdResponse
{
    public GetTodoByIdResponse(Todo? todo)
    {
        Todo = todo;
    }

    public GetTodoByIdResponse()
    {
        Todo = null;
    }

    public Todo? Todo { get; init; }
}
