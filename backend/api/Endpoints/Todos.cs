using AutoMapper;
using FluentValidation;
using MassTransit;
using TodoChallenge.Contracts.Entities;
using TodoChallenge.Contracts.Requests;

namespace TodoChallenge.Api.Endpoints;

internal class TodosEndpoint { }

internal static class Todos
{
    public static void AddTodosEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet(
            "/todos",
            async (IRequestClient<GetAllTodosRequest> requestor, IMapper mapper) =>
            {
                var response = await requestor.GetResponse<GetAllTodosResponse>(new GetAllTodosRequest());
                if (response is null)
                    return Results.Problem("Internal error");

                var dto = mapper.Map<IEnumerable<TodoDTO>>(response.Message.Todos);

                return Results.Ok(dto);
            }
        );

        routes.MapGet(
            "/todos/complete",
            async (IRequestClient<GetCompletedTodosRequest> requestor, IMapper mapper) =>
            {
                var response = await requestor.GetResponse<GetCompletedTodosResponse>(new GetCompletedTodosRequest());
                if (response is null)
                    return Results.Problem("Internal error");

                var dto = mapper.Map<IEnumerable<TodoDTO>>(response.Message.Todos);

                return Results.Ok(dto);
            }
        );

        routes.MapGet(
            "/todos/{id}",
            async (int id, IRequestClient<GetTodoByIdRequest> requestor, IMapper mapper) =>
            {
                var response = await requestor.GetResponse<GetTodoByIdResponse>(new GetTodoByIdRequest(id));
                if (response is null)
                    return Results.Problem("Internal error");

                var dto = mapper.Map<TodoDTO>(response.Message.Todo);

                return dto is null ? Results.NotFound() : Results.Ok(dto);
            }
        );

        routes.MapPost(
            "/todos",
            async (TodoDTO toInsert, IValidator<ITodo> validator, IRequestClient<InsertTodoRequest> requestor, IMapper mapper, ILogger<TodosEndpoint> logger) =>
            {
                var validationResult = validator.Validate(toInsert, opts => opts.IncludeRuleSets("insert"));

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var todo = mapper.Map<Todo>(toInsert);

                var response = await requestor.GetResponse<InsertTodoResponse>(new InsertTodoRequest(todo));
                if (response is null)
                    return Results.Problem("Internal error");

                logger.LogDebug("{Todo} inserted", response.Message.Todo);
                var dto = mapper.Map<TodoDTO>(response.Message.Todo);
                var error = response.Message.Error;

                return string.IsNullOrWhiteSpace(error) ? Results.Created($"/todos/{todo.Id}", dto) : Results.BadRequest(error);
            }
        );

        routes.MapPut(
            "/todos/{id}",
            async (
                int id,
                TodoDTO toUpdate,
                IValidator<ITodo> validator,
                IRequestClient<UpdateTodoRequest> requestor,
                IMapper mapper,
                ILogger<TodosEndpoint> logger
            ) =>
            {
                toUpdate = toUpdate with { Id = id };

                var validationResult = validator.Validate(toUpdate, opts => opts.IncludeRuleSets("insert", "update"));

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var todo = mapper.Map<Todo>(toUpdate);

                var response = await requestor.GetResponse<UpdateTodoResponse>(new UpdateTodoRequest(todo));
                if (response is null)
                    return Results.Problem("Internal error");

                logger.LogDebug("{Todo} updated", response.Message.Todo);
                var dto = mapper.Map<TodoDTO>(response.Message.Todo);
                var error = response.Message.Error;

                return string.IsNullOrWhiteSpace(error) ? Results.Ok(dto) : Results.BadRequest(error);
            }
        );

        routes.MapDelete(
            "/todos/{id}",
            async (int id, IRequestClient<DeleteTodoRequest> requestor, IMapper mapper) =>
            {
                var response = await requestor.GetResponse<DeleteTodoResponse>(new DeleteTodoRequest(id));
                if (response is null)
                    return Results.Problem("Internal error");

                var error = response.Message.Error;

                return string.IsNullOrWhiteSpace(error) ? Results.Ok() : Results.BadRequest(error);
            }
        );
    }
}
