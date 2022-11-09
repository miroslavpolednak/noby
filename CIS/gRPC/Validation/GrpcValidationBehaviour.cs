using FluentValidation;
using MediatR;

namespace CIS.Infrastructure.gRPC.Validation;

public class GrpcValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, Core.Validation.IValidatableRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public GrpcValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationFailures = _validators
            .Select(validator => validator.ValidateAsync(request, cancellationToken))
            .SelectMany(validationResult => validationResult.Result.Errors)
            .Where(validationFailure => validationFailure != null)
            .ToList();

        if (validationFailures.Any())
        {
            string message = string.Join("; ", validationFailures.Select(t => t.ErrorMessage));
            throw new CIS.Core.Exceptions.CisValidationException(validationFailures.Select(t => (Key: t.ErrorCode, Message: t.ErrorMessage)), message);
        }
        
        return next();
    }
}