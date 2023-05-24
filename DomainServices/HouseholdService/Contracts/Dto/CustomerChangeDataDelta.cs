namespace DomainServices.HouseholdService.Contracts.Dto;

/// <summary>
/// Vim ze je to takto blbe na dvou mistech, ale nechce se mi sdilet ty objekty mezi DS a FE API. Museli by se v tom zaroven resit OpenApi popisky, nekter subdto se na FE API pouzivaji na vice mistech... byl by to bordel.
/// </summary>
public class CustomerChangeDataDelta
{
    public NaturalPersonDelta? NaturalPerson { get; set; }
    public IdentificationDocumentFull? IdentificationDocument { get; set; }
    public List<CIS.Foms.Types.Address>? Addresses { get; set; }
    public LegalCapacityItem? LegalCapacity { get; set; }
    public bool? IsBrSubscribed { get; set; }
    public bool? HasRelationshipWithKB { get; set; }
    public bool? HasRelationshipWithKBEmployee { get; set; }
    public bool? HasRelationshipWithCorporate { get; set; }
    public bool? IsPoliticallyExposed { get; set; }
    public bool? IsUSPerson { get; set; }

    public class NaturalPersonDelta
    {
        public string? BirthNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? DegreeBeforeId { get; set; }
        public int? DegreeAfterId { get; set; }
        public string? BirthName { get; set; }
        public string? PlaceOfBirth { get; set; }
        public int? BirthCountryId { get; set; }
        public CIS.Foms.Enums.Genders Gender { get; set; }
        public int? MaritalStatusId { get; set; }
        public List<int>? CitizenshipCountriesId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? ProfessionCategoryId { get; set; }
        public int? ProfessionId { get; set; }
        public int? NetMonthEarningAmountId { get; set; }
        public int? NetMonthEarningTypeId { get; set; }
        public LegalCapacityItem? LegalCapacity { get; set; }
    }

    public class LegalCapacityItem
    {
        public int? RestrictionTypeId { get; set; }
        public DateTime? RestrictionUntil { get; set; }
    }

    public class IdentificationDocumentFull
    {
        public int IssuingCountryId { get; set; }
        public string IssuedBy { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
        public DateTime IssuedOn { get; set; }
        public string? RegisterPlace { get; set; }
        public int? IdentificationDocumentTypeId { get; set; }
        public string Number { get; set; } = string.Empty;
    }
}
