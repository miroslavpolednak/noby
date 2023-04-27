using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.CompleteTask;

internal class CompleteTaskHandler : IRequestHandler<CompleteTaskRequest>
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
            TaskIdSb = request.TaskIdSb,
            TaskUserResponse = request.TaskUserResponse,
            TaskDocumentIds = request.TaskDocumentIds
        };

        var responseCode = await _sbWebApiClient.CompleteTask(sbRequest, cancellationToken);
        
        switch (responseCode)
        {
            case 0:
                return;
            case 2:
                throw new CisValidationException(ErrorCodeMapper.TaskIdNotFound, ErrorCodeMapper.GetMessage(ErrorCodeMapper.TaskIdNotFound, request.TaskIdSb));
            default:
                throw new CisException(BaseCisException.UnknownExceptionCode, $"SbWebApi returned error code {responseCode}");
        }
    }
}