namespace TodoChallenge.Contracts.Entities;

public interface ITodo
{
    int Id { get; }
    string? Description { get; }
    bool IsComplete { get; }
    DateTime UpdatedAt { get; }
}

public record class Todo : ITodo
{
    public Todo() { }

    public int Id { get; set; }
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime UpdatedAt { get; set; }
}
