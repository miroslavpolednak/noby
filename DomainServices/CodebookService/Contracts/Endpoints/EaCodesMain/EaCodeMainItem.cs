using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.EaCodesMain;

[DataContract]
public class EaCodeMainItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string DescriptionForClient { get; set; }

    [DataMember(Order = 4)]
    public string Category { get; set; }

    [DataMember(Order = 5)]
    public string KindKb { get; set; }

    [DataMember(Order = 6)]
    public bool IsVisibleForKb { get; set; }

    [DataMember(Order = 7)]
    public bool IsInsertingAllowedNoby { get; set; }

    [DataMember(Order = 8)]
    public bool IsValid { get; set; }

    [DataMember(Order = 9)]
    public bool IsFormIdRequested { get; set; }
}
