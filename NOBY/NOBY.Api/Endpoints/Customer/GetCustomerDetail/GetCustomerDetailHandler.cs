using DomainServices.CustomerService.Clients.v1;
using NOBY.Services.Customer;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail;

internal sealed class GetCustomerDetailHandler(ICustomerServiceClient _customerService)
        : IRequestHandler<GetCustomerDetailRequest, CustomerGetCustomer>
{
    public async Task<CustomerGetCustomer> Handle(GetCustomerDetailRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = await _customerService.GetCustomerDetail(request.Identity, cancellationToken);

        CustomerNaturalPersonModel person = new();
        result.NaturalPerson?.FillResponseDto(person);
        person.IsBrSubscribed = result.NaturalPerson?.IsBrSubscribed;
    
        return new CustomerGetCustomer
        {
            NaturalPerson = person,
            JuridicalPerson = null,
            LegalCapacity = result.NaturalPerson?.LegalCapacity is null ? null : new CustomerLegalCapacityItem
            {
                RestrictionTypeId = result.NaturalPerson.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = result.NaturalPerson.LegalCapacity.RestrictionUntil
            },
            IdentificationDocument = result.IdentificationDocument?.ToResponseDto(),
            Contacts = result.Contacts?.ToResponseDto(),
            Addresses = result.Addresses?.Where(t => t.AddressTypeId != (int)AddressTypes.Other)
                              .Select(t => (SharedTypesAddress)t!).ToList()
        };
    }
}