namespace DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes;

[DataContract]
public class LoanPurposesItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    public int? MandantId { get; set; }


    [DataMember(Order = 4)]
    public List<int> ProductTypeIds { get; set; }


    [DataMember(Order = 5)]
    public int Order { get; set; }


    [DataMember(Order = 6)]
    // TODO: ačkoliv se to nemá propagovat na FE API, používá to aktuálně RIP na CodebooksAPI !? 
    // [JsonIgnore] 
    public int? C4mId { get; set; }


    [DataMember(Order = 7)]
    public bool IsValid { get; set; }
}