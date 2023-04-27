namespace DomainServices.CodebookService.Contracts.Endpoints.GetGeneralDocumentList;

[DataContract]
public class GetGeneralDocumentListItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    public string Filename { get; set; }


    [DataMember(Order = 4)]
    public string Format { get; set; }

}
