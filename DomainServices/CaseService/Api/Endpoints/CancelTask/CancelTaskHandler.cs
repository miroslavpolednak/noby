using CIS.Core.Exceptions.ExternalServices;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _sbWebApi.CancelTask(request.TaskIdSB, cancellationToken);
        }
        catch (CisExtServiceValidationException ex) when (ex.Errors.Any(t => t.ExceptionCode == "2"))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.TaskIdNotFound, request.TaskIdSB);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ISbWebApiClient _sbWebApi;

    public CancelTaskHandler(ISbWebApiClient sbWebApi)
    {
        _sbWebApi = sbWebApi;
    }
}
