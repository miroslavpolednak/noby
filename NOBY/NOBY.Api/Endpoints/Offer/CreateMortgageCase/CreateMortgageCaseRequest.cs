﻿using NOBY.Infrastructure.Swagger;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

[RollbackDescription("- maže domácnost s customerOnSA<br/>- maže SalesArrangement<br/>- maže Case")]
public sealed class CreateMortgageCaseRequest
    : IRequest<CreateMortgageCaseResponse>, CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable
{
    /// <summary>
    /// ID simulace ze ktere se ma vytvorit Case.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Identifikovany klient.
    /// </summary>
    public SharedTypes.Types.CustomerIdentity? Identity { get; set; }

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

    public NOBY.Dto.ContactsDto? OfferContacts { get; set; }
}
