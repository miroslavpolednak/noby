namespace DomainServices.CodebookService.Contracts.Endpoints.Developers;

[DataContract]
public class DeveloperItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    public string Cin { get; set; }


    [DataMember(Order = 4)]
    public bool IsValid { get; set; }


    [DataMember(Order = 5)]
    public int Status { get; set; }


    [DataMember(Order = 6)]
    public int BenefitPackage { get; set; }


    [DataMember(Order = 7)]
    public bool IsBenefitValid { get; set; }


    [DataMember(Order = 8)]
    public string BenefitsBeyondPackage { get; set; }
}