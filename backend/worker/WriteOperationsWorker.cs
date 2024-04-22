using MassTransit;
using TodoChallenge.Contracts.Requests;

namespace TodoChallenge.Worker;

public class WriteOperationsWorker
    : BackgroundService,
        IConsumer<InsertTodoRequest>,
        IConsumer<UpdateTodoRequest>,
        IConsumer<DeleteTodoRequest>
{
    private readonly ILogger<ReadOperationsWorker> _logger;
    private readonly ApplicationDbContext _db;

    public WriteOperationsWorker(ILogger<ReadOperationsWorker> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task Consume(ConsumeContext<InsertTodoRequest> context)
    {
        _logger.LogDebug("Received {Request} request", nameof(InsertTodoRequest));
        try
        {
            var todo = context.Message.Todo;

            await _db.Todos.AddAsync(todo);
            await _db.SaveChangesAsync();

            await context.RespondAsync(new InsertTodoResponse(todo));

            _logger.LogDebug("Responded with {Todo} todo", todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing {Request} request",
                nameof(InsertTodoRequest)
            );
            await context.RespondAsync(new InsertTodoResponse(ex.Message));
        }
    }

    public async Task Consume(ConsumeContext<UpdateTodoRequest> context)
    {
        _logger.LogDebug("Received {Request} request", nameof(UpdateTodoRequest));
        try
        {
            var todo = context.Message.Todo;

            _db.Todos.Update(todo);
            await _db.SaveChangesAsync();

            await context.RespondAsync(new UpdateTodoResponse(todo));

            _logger.LogDebug("Responded with {Todo} todo", todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing {Request} request",
                nameof(UpdateTodoRequest)
            );
            await context.RespondAsync(new UpdateTodoResponse(ex.Message));
        }
    }

    public async Task Consume(ConsumeContext<DeleteTodoRequest> context)
    {
        _logger.LogDebug("Received {Request} request", nameof(DeleteTodoRequest));
        try
        {
            var todo = await _db.Todos.FindAsync(context.Message.Id);
            if (todo is null)
            {
                _logger.LogWarning("Todo with id {Id} not found", context.Message.Id);
                await context.RespondAsync(new DeleteTodoResponse("Todo not found"));
                return;
            }

            // pt-br: em uma aplicacao real, provavelmente seria melhor um soft delete
            // en-us: in a real application, it would probably be better a soft delete
            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync();

            await context.RespondAsync(new DeleteTodoResponse());

            _logger.LogDebug("Responded with {Todo} todo", todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing {Request} request",
                nameof(DeleteTodoRequest)
            );
            await context.RespondAsync(new DeleteTodoResponse(ex.Message));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "{Worker} running at: {time}",
                nameof(WriteOperationsWorker),
                DateTimeOffset.Now
            );
            await Task.Delay(10 * 1000, stoppingToken);
        }
    }
}
