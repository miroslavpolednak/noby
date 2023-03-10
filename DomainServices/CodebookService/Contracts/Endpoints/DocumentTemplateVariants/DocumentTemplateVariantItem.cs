namespace DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVariants;

[DataContract]
public class DocumentTemplateVariantItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int DocumentTemplateVersionId { get; set; }

    [DataMember(Order = 3)]
    public string DocumentVariant { get; set; }

    [DataMember(Order = 4)]
    public string Description { get; set; }
}