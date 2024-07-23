namespace NOBY.ApiContracts;

public partial class WorkflowCancelTaskRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public long TaskId { get; private set; }

    public WorkflowCancelTaskRequest InfuseId(long caseId, long taskId)
    {
        TaskId = taskId;
        CaseId = caseId;
        return this;
    }
}
