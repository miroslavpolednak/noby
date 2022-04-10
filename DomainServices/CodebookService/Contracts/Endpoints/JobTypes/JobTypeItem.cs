namespace DomainServices.CodebookService.Contracts.Endpoints.JobTypes;

[DataContract]
public class JobTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public bool IsDefault { get; set; }

}