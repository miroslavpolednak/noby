using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class CreateCaseRequest
    : IRequest<CreateCaseResponse>, IValidatableRequest
{
    public int OfferId { get; set; }

    public CIS.Core.Types.CustomerIdentity? Customer { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }
}
