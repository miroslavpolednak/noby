using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Workflow.CancelTask;

public sealed class CancelTaskRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal long TaskId;

    internal CancelTaskRequest InfuseId(long caseId, long taskId)
    {
        TaskId = taskId;
        CaseId = caseId;
        return this;
    }

    public int? TaskIdSB { get; set; }
}
