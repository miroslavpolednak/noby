using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.V1;
using DomainServices.DocumentOnSAService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.v1.CompleteTask;

internal sealed class CompleteTaskHandler(
    ISbWebApiClient _sbWebApiClient,
    IDocumentOnSAServiceClient _documentOnSAService)
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

        if (request.TaskTypeId == (int)WorkflowTaskTypes.Signing)
        {
            sbRequest.Metadata.Add("ukol_podpis_odpoved_typ", (request.TaskResponseTypeId ?? 0).ToString(CultureInfo.InvariantCulture));
            sbRequest.Metadata.Add("ukol_podpis_zpusob_ukonceni", (request.CompletionTypeId ?? 0).ToString(CultureInfo.InvariantCulture));
        }
        else if (request.TaskTypeId == (int)WorkflowTaskTypes.RetentionRefixation) // Retence
        {
            sbRequest.Metadata.Add("ukol_retence_priprava_zpusob_uk", (request.CompletionTypeId ?? 0).ToString(CultureInfo.InvariantCulture));
        }

        await _sbWebApiClient.CompleteTask(sbRequest, cancellationToken);

        //// This is not working correctly so is temporarily turn off.
        //if (request.TaskTypeId == 6)
        //{
        //    await _documentOnSAService.SetProcessingDateInSbQueues(request.TaskId, request.CaseId, cancellationToken);
        //}

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static string getTaskUserResponseDef(int taskTypeId)
        => taskTypeId switch
        {
            1 => "ukol_dozadani_odpoved_oz",
            6 => "ukol_podpis_odpoved_text",
            9 => "ukol_retence_pozadavek",
            _ => throw new NotImplementedException()
        };
}