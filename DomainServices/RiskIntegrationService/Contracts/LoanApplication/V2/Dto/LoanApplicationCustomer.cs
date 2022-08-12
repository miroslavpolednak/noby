namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationCustomer
{
    [ProtoMember(1)]
    public int? InternalCustomerId { get; set; }

    [ProtoMember(2)]
    public string PrimaryCustomerId { get; set; }

    [ProtoMember(3)]
    public bool IsGroupEmployee { get; set; }

    [ProtoMember(4)]
    public bool SpecialRelationsWithKB { get; set; }

    [ProtoMember(5)]
    public string? BirthNumber { get; set; }

    [ProtoMember(6)]
    public int? CustomerRoleId { get; set; }

    [ProtoMember(7)]
    public string? Firstname { get; set; }

    [ProtoMember(8)]
    public string? Surname { get; set; }

    [ProtoMember(9)]
    public string? BirthName { get; set; }

    [ProtoMember(10)]
    public DateTime? BirthDate { get; set; }

    [ProtoMember(11)]
    public string? BirthPlace { get; set; }

    [ProtoMember(12)]
    public int? GenderId { get; set; }

    [ProtoMember(13)]
    public int? MaritalStateId { get; set; }

    [ProtoMember(14)]
    public int? EducationLevelId { get; set; }

    [ProtoMember(15)]
    public int? AcademicDegreeBeforeId { get; set; }

    [ProtoMember(16)]
    public string? MobilePhoneNumber { get; set; }

    [ProtoMember(17)]
    public bool HasEmail { get; set; }

    [ProtoMember(18)]
    public bool IsPartner { get; set; }

    [ProtoMember(19)]
    public int? HousingConditionId { get; set; }

    [ProtoMember(20)]
    public bool Taxpayer { get; set; }

    [ProtoMember(21)]
    public Shared.AddressDetail? Address { get; set; }

    public string Income { get; set; }

}
