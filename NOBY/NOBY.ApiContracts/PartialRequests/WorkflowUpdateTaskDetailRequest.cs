namespace NOBY.ApiContracts;

public partial class WorkflowUpdateTaskDetailRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public long TaskId { get; private set; }

    public WorkflowUpdateTaskDetailRequest InfuseIds(long caseId, long taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
        return this;
    }
}
