using FluentValidation;
using MediatR;

namespace CIS.Infrastructure.WebApi.Validation;

public class WebApiValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, Core.Validation.IValidatableRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public WebApiValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationFailures = _validators
            .Select(validator => validator.ValidateAsync(request, cancellationToken))
            .SelectMany(validationResult => validationResult.Result.Errors)
            .Where(validationFailure => validationFailure != null)
            .ToList();

        if (validationFailures.Any())
            throw new CIS.Core.Exceptions.CisValidationException(validationFailures.Select(t => (t.PropertyName, t.ErrorMessage)));
 
        return next();
    }
}