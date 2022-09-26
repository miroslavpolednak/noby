using System;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.DocumentFileTypes;

[DataContract]
public class DocumentFileTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.DocumentFileType EnumValue { get; set; }

    [DataMember(Order = 3)]
    [JsonIgnore]
    public string DocumenFileType { get; set; }

    [DataMember(Order = 4)]
    public bool IsPrintingSupported { get; set; }

}
