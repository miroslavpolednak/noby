using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(CancelTaskRequest request, CancellationToken cancellation)
    {
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ISbWebApiClient _sbWebApi;

    public CancelTaskHandler(ISbWebApiClient sbWebApi)
    {
        _sbWebApi = sbWebApi;
    }
}
