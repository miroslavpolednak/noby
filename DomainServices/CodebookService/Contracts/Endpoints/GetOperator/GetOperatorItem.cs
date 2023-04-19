namespace DomainServices.CodebookService.Contracts.Endpoints.GetOperator;

[DataContract]
public class GetOperatorItem
{
    [DataMember(Order = 1)]
    public string PerformerLogin { get; set; }

    [DataMember(Order = 2)]
    public string PerformerName { get; set; }
}
