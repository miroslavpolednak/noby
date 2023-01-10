using FluentValidation;

namespace CIS.Infrastructure.CisMediatR;

/// <summary>
/// MediatR pipeline, která přidává do flow requestu FluentValidation.
/// </summary>
/// <remarks>
/// Pokud v rámci pipeline handleru vrátí FluentValidation chyby, vyhodíme vyjímku CisValidationException a ukončí se flow requestu.
/// </remarks>
public sealed class GrpcValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, CIS.Core.Validation.IValidatableRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public GrpcValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
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
            throw new Core.Exceptions.CisValidationException(validationFailures.Select(t => new CIS.Core.Exceptions.CisExceptionItem(t.ErrorCode, t.ErrorMessage)));
        }
        
        return next();
    }
}