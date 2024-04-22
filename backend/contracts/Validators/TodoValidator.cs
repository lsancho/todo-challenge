using FluentValidation;
using TodoChallenge.Contracts.Entities;

namespace TodoChallenge.Contracts.Validators;

public class TodoValidator : AbstractValidator<ITodo>
{
    public TodoValidator()
    {
        RuleSet(
            "insert",
            () =>
            {
                RuleFor(e => e.Description)
                    .NotEmpty()
                    .WithMessage("Is required.")
                    .MinimumLength(10)
                    .WithMessage("Must be at least 10 characters long.")
                    .WithName("description");

                RuleFor(e => e.UpdatedAt)
                    .NotEmpty()
                    .WithMessage("Is required.")
                    .GreaterThan(new DateTime(2024, 01, 01))
                    .WithMessage("Must be greater than 2024-01-01.")
                    .WithName("updated_at");
            }
        );

        RuleSet(
            "update",
            () =>
            {
                RuleFor(e => e.Id)
                    .NotEmpty()
                    .WithMessage("Is required.")
                    .GreaterThan(0)
                    .WithMessage("Must be greater than 0.")
                    .WithName("id");
            }
        );
    }
}
