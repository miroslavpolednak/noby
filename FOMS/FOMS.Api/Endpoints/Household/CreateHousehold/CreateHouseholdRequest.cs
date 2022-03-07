using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Household.CreateHousehold;

public class CreateHouseholdRequest
    : IRequest<Dto.HouseholdInList>, IValidatableRequest
{
    /// <summary>
    /// ID pripadu
    /// </summary>
    public int SalesArrangementId { get; set; }

    /// <summary>
    /// ID typu domacnosti. Ciselnik HouseholdTypes
    /// </summary>
    /// <example>1</example>
    public int HouseholdTypeId { get; set; }
}
