using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoChallenge.Contracts.Requests;

namespace TodoChallenge.Worker;

public class ReadOperationsWorker
    : BackgroundService,
        IConsumer<GetAllTodosRequest>,
        IConsumer<GetTodoByIdRequest>,
        IConsumer<GetCompletedTodosRequest>
{
    private readonly ILogger<ReadOperationsWorker> _logger;
    private readonly ApplicationDbContext _db;

    public ReadOperationsWorker(ILogger<ReadOperationsWorker> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task Consume(ConsumeContext<GetAllTodosRequest> context)
    {
        _logger.LogDebug("Received {Request} request", nameof(GetAllTodosRequest));
        try
        {
            var todos = await _db.Todos.ToListAsync();

            await context.RespondAsync(new GetAllTodosResponse(todos));
            _logger.LogDebug("Responded with {Todos} todos", todos.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing {Request} request",
                nameof(GetAllTodosRequest)
            );
            await context.RespondAsync(new GetAllTodosResponse());
        }
    }

    public async Task Consume(ConsumeContext<GetTodoByIdRequest> context)
    {
        _logger.LogDebug("Received {Request} request", nameof(GetTodoByIdRequest));

        try
        {
            var todo = await _db.Todos.FindAsync(context.Message.Id);

            await context.RespondAsync(new GetTodoByIdResponse(todo));

            _logger.LogDebug("Responded with {Todo} todo", todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing {Request} request",
                nameof(GetTodoByIdRequest)
            );
            await context.RespondAsync(new GetTodoByIdResponse());
        }
    }

    public async Task Consume(ConsumeContext<GetCompletedTodosRequest> context)
    {
        _logger.LogDebug("Received {Request} request", nameof(GetCompletedTodosRequest));

        try
        {
            var todos = await _db.Todos.Where(todo => todo.IsComplete).ToListAsync();

            await context.RespondAsync(new GetCompletedTodosResponse(todos));

            _logger.LogDebug("Responded with {Todos} todos", todos.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing {Request} request",
                nameof(GetCompletedTodosRequest)
            );
            await context.RespondAsync(new GetCompletedTodosResponse());
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "{Worker} running at: {time}",
                nameof(ReadOperationsWorker),
                DateTimeOffset.Now
            );
            await Task.Delay(10 * 1000, stoppingToken);
        }
    }
}
