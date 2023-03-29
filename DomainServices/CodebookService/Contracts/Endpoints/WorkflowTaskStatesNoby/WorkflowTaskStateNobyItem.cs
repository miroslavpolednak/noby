namespace DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStatesNoby;

[DataContract]
public class WorkflowTaskStateNobyItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string Filter { get; set; }

    [DataMember(Order = 4)]
    public string Indicator { get; set; }
}