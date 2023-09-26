﻿using NOBY.Api.Endpoints.Customer.CreateCustomer.Dto;
using NOBY.Api.Endpoints.Customer.GetCustomerDetail.Dto;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

public sealed class CreateCustomerResponse
{
    /// <summary>
    /// Vstupní data se liší od dat z KB CM
    /// </summary>
    public bool IsInputDataDifferent { get; set; }

    /// <summary>
    /// Klient byl při založení ztotožněn v základních registrech
    /// </summary>
    public bool IsVerified { get; set; }

    public ResultCode ResultCode { get; set; }

    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }

    public NaturalPersonModel? NaturalPerson { get; set; }

    public Shared.JuridicalPerson? JuridicalPerson { get; set; }

    public bool Updatable { get; set; }

    public Shared.LegalCapacityItem? LegalCapacity { get; set; }

    public List<SharedTypes.Types.Address>? Addresses { get; set; }

    public NOBY.Dto.ContactsDto? Contacts { get; set; }

    public NOBY.Dto.IdentificationDocumentFull? IdentificationDocument { get; set; }
}
