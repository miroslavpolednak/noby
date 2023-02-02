using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;

[DataContract]
public class DocumentTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.DocumentType EnumValue { get; set; }
    

    [DataMember(Order = 3)]
    public string Name { get; set; }


    [DataMember(Order = 4)]
    public string ShortName { get; set; }


    [DataMember(Order = 5)]
    public string FileName { get; set; }


    [DataMember(Order = 6)]
    public int? SalesArrangementTypeId { get; set; }


    [DataMember(Order = 7)]
    public bool IsValid { get; set; }


    [DataMember(Order = 8)]
    public int? EACodeMainId { get; set; }


    [DataMember(Order = 9)]
    public bool IsFormIdRequested { get; set; }

}
