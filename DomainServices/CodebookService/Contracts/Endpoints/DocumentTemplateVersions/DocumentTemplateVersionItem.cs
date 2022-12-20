namespace DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;

[DataContract]
public class DocumentTemplateVersionItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int DocumentTemplateTypeId { get; set; }

    [DataMember(Order = 3)]
    public string DocumentVersion { get; set; }

    [DataMember(Order = 4)]
    public bool IsValid { get; set; }
}
