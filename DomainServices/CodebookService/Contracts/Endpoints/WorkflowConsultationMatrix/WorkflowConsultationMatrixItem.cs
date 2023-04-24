namespace DomainServices.CodebookService.Contracts.Endpoints.WorkflowConsultationMatrix;

[DataContract]
public sealed class WorkflowConsultationMatrixItem
{
    [DataMember(Order = 1)]
    public int TaskSubtypeId { get; set; }

    [DataMember(Order = 2)]
    public string TaskSubtypeName { get; set; }

    [DataMember(Order = 3)]
    public List<WorkflowConsultationMatrixItemValidity> IsValidFor { get; set; }
}

[DataContract]
public sealed class WorkflowConsultationMatrixItemValidity
{
    [DataMember(Order = 1)]
    public int ProcessTypeId { get; set; }

    [DataMember(Order = 2)]
    public int ProcessPhaseId { get; set; }
}
