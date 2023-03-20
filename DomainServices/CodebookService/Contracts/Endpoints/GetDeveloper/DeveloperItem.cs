namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;

[DataContract]
public class DeveloperItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string Cin { get; set; }

    [DataMember(Order = 5)]
    public int StatusId { get; set; }

    [DataMember(Order = 6)]
    public string StatusText { get; set; }

    [DataMember(Order = 7)]
    public bool IsValid { get; set; }

    [DataMember(Order = 8)]
    public bool BenefitPackage { get; set; }

    [DataMember(Order = 9)]
    public bool IsBenefitValid { get; set; }

    [DataMember(Order = 10)]
    public bool BenefitsBeyondPackage { get; set; }
}