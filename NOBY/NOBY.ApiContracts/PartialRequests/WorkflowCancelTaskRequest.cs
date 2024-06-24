namespace NOBY.ApiContracts;

public partial class WorkflowCancelTaskRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId;

    [JsonIgnore]
    public long TaskId;

    public WorkflowCancelTaskRequest InfuseId(long caseId, long taskId)
    {
        TaskId = taskId;
        CaseId = caseId;
        return this;
    }
}
