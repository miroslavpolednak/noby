using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;

[DataContract]
public class WorkflowTaskStateItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    [JsonIgnore]
    public EWorkflowTaskStateFlag Flag { get; set; }

}


[System.Flags]
public enum EWorkflowTaskStateFlag : short
{
    Undefined = 0,
    Inactive = 1,
}