using Azure.Core;
using FluentValidation;
using MediatR;

namespace TaskManagement_CQRS_Pattern.Api.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // DI injects ALL validators that match this request type
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        // 1. Check if the request has a validator
        if (_validators.Any())
        {
            // 2. Wrap the request in a context FluentValidation understands
            var context = new ValidationContext<TRequest>(request);

            // 3. Run every validator and collect their results
            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, ct)));

            // 4. Gather all the failures (broken rules) into one list
            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            // 5. If any rule failed → stop here, throw. Handler NEVER runs.
            if (failures.Count != 0)
                throw new ValidationException(failures);
        }

        // 6. All good (or no validator) → run the actual handler
        return await next();
    }
}
