using CIS.Core.Exceptions;
using CIS.Infrastructure.CisMediatR.GrpcValidation;
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
            var customStateException = validationFailures.FirstOrDefault(t => t.CustomState is not null && t.CustomState is GrpcValidationBehaviorExeptionTypes);

            // pokud v customState najdu instanci nejake Exception, misto toho abych sel standardni CisValidationException, pouziji tuto exception
            if (customStateException is not null)
            {
                if (!int.TryParse(customStateException.ErrorMessage, out int errorCode))
                    errorCode = 0;

                switch ((GrpcValidationBehaviorExeptionTypes)customStateException.CustomState)
                {
                    case GrpcValidationBehaviorExeptionTypes.CisNotFoundException:
                        throw new CisNotFoundException(errorCode, customStateException.ErrorMessage);
                    case GrpcValidationBehaviorExeptionTypes.CisArgumentException:
                        throw new CisArgumentException(errorCode, customStateException.ErrorMessage);
                    case GrpcValidationBehaviorExeptionTypes.CisValidationException:
                        throw new CisArgumentException(errorCode, customStateException.ErrorMessage);
                }
            }

            throw new CisValidationException(validationFailures.Select(t => new CisExceptionItem(t.ErrorCode, t.ErrorMessage)));
        }
        
        return next();
    }
}