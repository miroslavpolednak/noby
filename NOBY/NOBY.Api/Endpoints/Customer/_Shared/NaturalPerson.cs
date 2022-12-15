namespace NOBY.Api.Endpoints.Customer.Shared;

public sealed class NaturalPerson
    : SharedDto.BaseNaturalPerson
{
    /// <summary>
    /// Stupeň vzdělání
    /// </summary>
    public int? EducationLevelId { get; set; }

    public int? ProfessionCategoryId { get; set; }

    public int? ProfessionId { get; set; }

    public int? NetMonthEarningAmountId { get; set; }

    public int? NetMonthEarningTypeId { get; set; }

    public LegalCapacityItem? LegalCapacity { get; set; }

    public TaxResidenceItem? TaxResidences { get; set; }
}
