using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateDraftRequest
    : IRequest<SaveCaseResponse>, IValidatableRequest
{
    public string? CustomerIdentity { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int OfferInstanceId { get; set; }

    public int? SalesArrangementId { get; set; }

    public CIS.Core.Types.CustomerIdentity? Customer 
    { 
        get => _customer ?? (string.IsNullOrEmpty(CustomerIdentity) ? null : (_customer = new CIS.Core.Types.CustomerIdentity(CustomerIdentity)));
    }
    private CIS.Core.Types.CustomerIdentity? _customer = null;
}
