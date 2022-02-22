using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

public sealed class CreateMortgageCaseRequest
    : IRequest<CreateMortgageCaseResponse>, IValidatableRequest
{
    /// <summary>
    /// ID simulace ze ktere se ma vytvorit Case.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Identifikovany klient.
    /// </summary>
    public CIS.Foms.Types.CustomerIdentity? Customer { get; set; }

    /// <summary>
    /// Jmeno klienta, pokud neni identifikovan.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Prijmeni klienta, pokud neni identifikovan.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narozeni klienta, pokud neni identifikovan.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
}
