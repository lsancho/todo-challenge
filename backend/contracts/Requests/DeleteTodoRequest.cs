namespace TodoChallenge.Contracts.Requests;

public record DeleteTodoRequest
{
    public DeleteTodoRequest() { }

    public DeleteTodoRequest(int id)
    {
        Id = id;
    }

    public int Id { get; init; }
}

public record DeleteTodoResponse
{
    public DeleteTodoResponse() { }

    public DeleteTodoResponse(string error)
    {
        Error = error;
    }

    public string? Error { get; init; }
}
