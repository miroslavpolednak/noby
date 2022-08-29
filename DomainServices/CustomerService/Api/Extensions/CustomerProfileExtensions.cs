using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Extensions;

public static class CustomerProfileExtensions
{
    public static async Task<TResult> CheckCustomerProfileResult<TResult>(this Task<IServiceCallResult> result)
        => await result switch
        {
            SuccessfulServiceCallResult<TResult> r => r.Model,
            ErrorServiceCallResult error => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, error.Errors[0].Message, error.Errors[0].Key),
            _ => throw new NotImplementedException()
        };
}