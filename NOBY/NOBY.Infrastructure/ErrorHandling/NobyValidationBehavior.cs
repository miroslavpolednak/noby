using FluentValidation;
using MediatR;

namespace NOBY.Infrastructure.ErrorHandling;

/// <summary>
/// MediatR pipeline, která přidává do flow requestu FluentValidation.
/// </summary>
/// <remarks>
/// Pokud v rámci pipeline handleru vrátí FluentValidation chyby, vyhodíme vyjímku CisValidationException a ukončí se flow requestu.
/// </remarks>
public sealed class NobyValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public NobyValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
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
            throw new NobyValidationException(validationFailures.Select(t =>
            {
                var code = string.IsNullOrEmpty(t.ErrorCode) ? NobyValidationException.DefaultExceptionCode : t.ErrorCode;
                if (!int.TryParse(code, out int x))
                    code = NobyValidationException.DefaultExceptionCode;
                
                ApiErrorItemServerity severity = t.Severity == Severity.Warning ? ApiErrorItemServerity.Warning : ApiErrorItemServerity.Error;

                return new ApiErrorItem(code, t.ErrorMessage, severity);
            }));
        }
        
        return next();
    }
}