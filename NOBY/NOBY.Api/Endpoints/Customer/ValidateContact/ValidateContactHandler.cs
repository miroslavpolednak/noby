using DomainServices.CustomerService.Clients.v1;
using DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.ValidateContact;

internal sealed class ValidateContactHandler: IRequestHandler<CustomerValidateContactRequest, CustomerValidateContactResponse>
{
    private readonly ICustomerServiceClient _customerService;
    
    public async Task<CustomerValidateContactResponse> Handle(CustomerValidateContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _customerService.ValidateContact(Map(request), cancellationToken);
        return Map(response);
    }

    private static DomainServices.CustomerService.Contracts.ValidateContactRequest Map(CustomerValidateContactRequest request) => new()
    {
        Contact = request.Contact,
        ContactType = Map(request.ContactType)
    };

    private static CustomerValidateContactResponse Map(DomainServices.CustomerService.Contracts.ValidateContactResponse response) => new()
    {
        IsContactValid = response.IsContactValid,
        ContactType = Map(response.ContactType),
        IsMobile = response.IsMobile
    };
    
    private static ContactType Map(EnumContactType contactType) => contactType switch
    {
        EnumContactType.Unknown => ContactType.Unknown,
        EnumContactType.Phone => ContactType.Phone,
        EnumContactType.Email => ContactType.Email,
        _ => throw new CisValidationException(11033, "ContactType has unexpected value.")
    };
    
    private static EnumContactType Map(ContactType contactType) => contactType switch
    {
        ContactType.Unknown =>EnumContactType.Unknown,
        ContactType.Phone => EnumContactType.Phone,
        ContactType.Email => EnumContactType.Email,
        _ => throw new ArgumentException("unknown contact type")
    };

    public ValidateContactHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}