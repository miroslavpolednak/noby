using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Household.SaveHouseholds;

public class SaveHouseholdsRequest
    : IRequest<List<int>>, IValidatableRequest
{
    /// <summary>
    /// ID sales arrangementu
    /// </summary>
    public int SalesArrangementId { get; set; }
    
    /// <summary>
    /// Seznam vsech zadanych domacnosti
    /// </summary>
    public List<Dto.Household>? Households { get; set; }
}