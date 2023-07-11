using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.CompleteTask;

internal sealed class CompleteTaskHandler 
    : IRequestHandler<CompleteTaskRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(CompleteTaskRequest request, CancellationToken cancellationToken)
    {
        var sbRequest = new ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest
        {
            TaskIdSb = request.TaskIdSb,
            Metadata = new Dictionary<string, string>
            {
                { "ukol_mandant", "2" },
                { "ukol_uver_id", request.CaseId.ToString(CultureInfo.InvariantCulture) },
                { "wfl_refobj_dokumenty", string.Join(",", request.TaskDocumentIds) },
                { getTaskUserResponseDef(request.TaskTypeId), request.TaskUserResponse ?? "" }
            }
        };

        if (request.TaskTypeId == 6)
        {
            sbRequest.Metadata.Add("ukol_podpis_odpoved_typ", (request.TaskResponseTypeId ?? 0).ToString(CultureInfo.InvariantCulture));
        }

        await _sbWebApiClient.CompleteTask(sbRequest, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static string getTaskUserResponseDef(int taskTypeId)
        => taskTypeId switch
        {
            1 => "ukol_dozadani_odpoved_oz",
            6 => "ukol_podpis_odpoved_text",
            _ => throw new NotImplementedException()
        };

    private readonly ISbWebApiClient _sbWebApiClient;

    public CompleteTaskHandler(ISbWebApiClient sbWebApiClient)
    {
        _sbWebApiClient = sbWebApiClient;
    }
}