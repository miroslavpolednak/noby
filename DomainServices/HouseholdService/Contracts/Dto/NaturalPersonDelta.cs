using CIS.Foms.Enums;

namespace DomainServices.HouseholdService.Contracts.Dto;

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
    public Genders Gender { get; set; }
    public int? MaritalStatusId { get; set; }
    public List<int>? CitizenshipCountriesId { get; set; }
    public int? EducationLevelId { get; set; }
    public int? ProfessionCategoryId { get; set; }
    public int? ProfessionId { get; set; }
    public int? NetMonthEarningAmountId { get; set; }
    public int? NetMonthEarningTypeId { get; set; }
    public LegalCapacityDelta? LegalCapacity { get; set; }
    public TaxResidenceDelta? TaxResidences { get; set; }

    public class LegalCapacityDelta
    {
        public int? RestrictionTypeId { get; set; }
        public DateTime? RestrictionUntil { get; set; }
    }

    public class TaxResidenceDelta
    {
        public DateTime? ValidFrom { get; set; }

        public List<TaxResidenceItemDelta>? ResidenceCountries { get; set; }

        public class TaxResidenceItemDelta
        {
            public int? CountryId { get; set; }

            public string? Tin { get; set; }

            public string? TinMissingReasonDescription { get; set; }
        }
    }
}