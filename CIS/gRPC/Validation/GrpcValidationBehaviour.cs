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

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationFailures = _validators
            .Select(validator => validator.ValidateAsync(request, cancellationToken))
            .SelectMany(validationResult => validationResult.Result.Errors)
            .Where(validationFailure => validationFailure != null)
            .ToList();

        if (validationFailures.Any())
        {
            var collection = new GrpcErrorCollection(validationFailures.Select(t => new GrpcErrorCollection.GrpcErrorCollectionItem(t.ErrorCode, t.ErrorMessage)));
            throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, "Validation errors occured", collection);
        }
        
        return next();
    }
}