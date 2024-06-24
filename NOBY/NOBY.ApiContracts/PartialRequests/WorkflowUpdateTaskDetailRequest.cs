namespace NOBY.ApiContracts;

public partial class WorkflowUpdateTaskDetailRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId;

    [JsonIgnore]
    public long TaskId;

    public WorkflowUpdateTaskDetailRequest InfuseIds(long caseId, long taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
        return this;
    }
}
