namespace DomainServices.CodebookService.Contracts.Endpoints.WorkflowProcessStatesNoby;

[DataContract]
public class WorkflowProcessStateNobyItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string Indicator { get; set; }
}