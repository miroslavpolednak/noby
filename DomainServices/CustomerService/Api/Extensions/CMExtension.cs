using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using Grpc.Core;

namespace DomainServices.CustomerService.Api;

internal static class CMExtension
{
    public static T ToCMResult<T>(this IServiceCallResult result)
    {
        CheckCMResult(result);

        var ret = result switch
        {
            //SuccessfulServiceCallResult<CustomerManagement.CMWrapper.CustomerSearchResult> r
            //=> r.Model,
            SuccessfulServiceCallResult <T> r 
            => r.Model,
            _ => throw new NotImplementedException()
        };

        return ret;
    }

    public static void CheckCMResult(this IServiceCallResult result)
    {

        switch (result)
        {
            case SuccessfulServiceCallResult<CustomerManagement.CMWrapper.ApiException<CustomerManagement.CMWrapper.Error>> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"Incorrect inputs to CustomerManagement: {r.Model.Result.Message}", 10011);
            case SuccessfulServiceCallResult<CustomerManagement.CMWrapper.Error> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"CustomerManagement error: {r.Model.Message}", 10011, new()
                {
                    ("cmerrorcode", r.Model.Code),
                    ("cmerrortext", r.Model.Message)
                });
            case SuccessfulServiceCallResult<MpHome.MpHomeWrapper.ApiException> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to CustomerManagement", 10011, new()
                {
                    ("cmerrorcode", r.Model.StatusCode.ToString()),
                    ("cmerrortext", r.Model.Message)
                });
            case ErrorServiceCallResult err:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key);
        }
    }
}
