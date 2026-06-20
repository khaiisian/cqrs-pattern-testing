using FluentValidation;

namespace TaskManagement_CQRS_Pattern.Api.Features.Tasks.Commands.CreateTasks;

public class CreateTaskCommandValidator: AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must be 200 characters or fewer.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must be 1000 characters or fewer.");
    }
}
