namespace DomainServices.CodebookService.Contracts.Endpoints.DocumentOnSATypes;

[DataContract]
public class DocumentOnSATypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    public int? SalesArrangementTypeId { get; set; }


    [DataMember(Order = 4)]
    public int FormTypeId { get; set; }
}
