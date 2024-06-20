﻿using NOBY.Dto.Customer;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail;

public sealed class GetCustomerDetailResponse
{
    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }
    
    public Dto.NaturalPersonModel? NaturalPerson { get; set; }
    
    public JuridicalPerson? JuridicalPerson { get; set; }
    
    public LegalCapacityItem? LegalCapacity { get; set; }
    
    public List<SharedTypes.Types.Address>? Addresses { get; set; }
    
    public NOBY.Dto.ContactsConfirmedDto? Contacts { get; set; }
    
    public NOBY.Dto.IdentificationDocumentFull? IdentificationDocument { get; set; }
}