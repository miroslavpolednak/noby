namespace DomainServices.CodebookService.Contracts.Endpoints.Genders;

[DataContract]
public class GenderItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public CIS.Foms.Enums.Genders MpHomeCode { get; set; }
    
    [DataMember(Order = 4)]
    public int KonsDBCode { get; set; }

    [DataMember(Order = 5)]
    public string KbCmCode { get; set; }

    [DataMember(Order = 6)]
    public string StarBuildJsonCode { get; set; }
}