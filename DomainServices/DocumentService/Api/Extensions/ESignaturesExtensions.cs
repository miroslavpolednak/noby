using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.DocumentService.Api;

internal static class ESignaturesExtensions
{

    public static void CheckESignaturesResult(this IServiceCallResult result)
    {
        switch (result)
        {
            case SuccessfulServiceCallResult<ESignatures.ESignaturesWrapper.ApiException> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to ESignatures", 10011, new()
                {
                    ("esignatureserrorcode", r.Model.StatusCode.ToString()),
                    ("esignatureserrortext", r.Model.Message)
                });
            case ErrorServiceCallResult err:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key);
        }
    }

    public static T ToESignaturesResult<T>(this IServiceCallResult result) {

        result.CheckESignaturesResult();

        var ret = result switch
        {
            SuccessfulServiceCallResult<T> r => r.Model,
            _ => throw new NotImplementedException()
        };

        return ret;
    }
}