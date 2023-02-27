﻿using NOBY.Api.Endpoints.Customer.GetDetail.Dto;

namespace NOBY.Api.Endpoints.Customer.Create;

public sealed class CreateResponse
{
    public string ResponseCode { get; set; } = string.Empty;

    /// <summary>
    /// Vstupní data se liší od dat z KB CM
    /// </summary>
    public bool IsInputDataDifferent { get; set; }

    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }

    public NaturalPersonModel? NaturalPerson { get; set; }

    public Shared.JuridicalPerson? JuridicalPerson { get; set; }

    public bool Updatable { get; set; }

    public bool IsLegallyIncapable { get; set; }

    public DateTime? LegallyIncapableToDate { get; set; }

    public List<CIS.Foms.Types.Address>? Addresses { get; set; }

    public SharedDto.ContactsDto? Contacts { get; set; }

    public SharedDto.IdentificationDocumentFull? IdentificationDocument { get; set; }
}
