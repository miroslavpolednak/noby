namespace NOBY.ApiContracts;

public partial class WorkflowCreateTaskRequest
    : IRequest<long>
{
    [JsonIgnore]
    public long CaseId;

    public WorkflowCreateTaskRequest InfuseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }
}
