using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.CompleteTask;

internal sealed class CompleteTaskHandler : IRequestHandler<CompleteTaskRequest>
{
    private readonly ISbWebApiClient _sbWebApiClient;

    public CompleteTaskHandler(ISbWebApiClient sbWebApiClient)
    {
        _sbWebApiClient = sbWebApiClient;
    }

    public async Task Handle(CompleteTaskRequest request, CancellationToken cancellationToken)
    {
        var sbRequest = new ExternalServices.SbWebApi.Dto.CompleteTaskRequest
        {
            CaseId = request.CaseId,
            TaskIdSb = request.TaskIdSb,
            TaskUserResponse = request.TaskUserResponse,
            TaskDocumentIds = request.TaskDocumentIds
        };

        await _sbWebApiClient.CompleteTask(sbRequest, cancellationToken);
    }
}