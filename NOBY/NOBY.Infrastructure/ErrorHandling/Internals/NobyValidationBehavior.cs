using FluentValidation;
using MediatR;

namespace NOBY.Infrastructure.ErrorHandling.Internals;

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
            var errors = validationFailures.Select(t =>
            {
                // v chybe je zadany errorCode. asi.
                if ((t.ErrorCode ?? "").Length == 5 
                    && int.TryParse(t.ErrorCode, out int errorCode)
                    && ErrorCodeMapper.Messages.ContainsKey(errorCode))
                {
                    // jedna se o chybu se zadanym kodem
                    return new ApiErrorItem(errorCode);
                }

                ApiErrorItemServerity severity = t.Severity == Severity.Warning ? ApiErrorItemServerity.Warning : ApiErrorItemServerity.Error;
                return new ApiErrorItem(NobyValidationException.DefaultExceptionCode, ErrorCodeMapper.Messages[NobyValidationException.DefaultExceptionCode].Message, t.ErrorMessage, severity);
            });

            throw new NobyValidationException(errors);
        }

        return next();
    }
}