namespace DomainServices.CodebookService.Contracts.Endpoints.RepaymentScheduleTypes;

[DataContract]
public class RepaymentScheduleTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Code { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }
}