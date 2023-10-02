﻿namespace NOBY.Api.Endpoints.Product.GetCustomersOnProduct;

public sealed class GetCustomersOnProductCustomer
{
    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Příjmení
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    public NOBY.Dto.IdentificationDocumentBase? IdentificationDocument { get; set; }
}
