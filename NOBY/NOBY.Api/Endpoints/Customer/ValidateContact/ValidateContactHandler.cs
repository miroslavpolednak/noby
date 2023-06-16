using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.ValidateContact;

internal sealed class ValidateContactHandler: IRequestHandler<ValidateContactRequest, ValidateContactResponse>
{
    private readonly ICustomerServiceClient _customerService;
    
    public async Task<ValidateContactResponse> Handle(ValidateContactRequest request, CancellationToken cancellationToken)
    {
        var response = await _customerService.ValidateContact(Map(request), cancellationToken);
        return Map(response);
    }

    private static DomainServices.CustomerService.Contracts.ValidateContactRequest Map(ValidateContactRequest request) => new()
    {
        Contact = request.Contact,
        ContactType = Map(request.ContactType)
    };

    private static ValidateContactResponse Map(DomainServices.CustomerService.Contracts.ValidateContactResponse response) => new()
    {
        IsContactValid = response.IsContactValid,
        ContactType = Map(response.ContactType)
    };
    
    private static ContactType Map(Dto.ContactType contactType) => contactType switch
    {
        Dto.ContactType.Unknown => ContactType.Unknown,
        Dto.ContactType.Phone => ContactType.Phone,
        Dto.ContactType.Email => ContactType.Email,
        _ => throw new ArgumentException()
    };
    
    private static Dto.ContactType Map(ContactType contactType) => contactType switch
    {
        ContactType.Unknown => Dto.ContactType.Unknown,
        ContactType.Phone => Dto.ContactType.Phone,
        ContactType.Email => Dto.ContactType.Email,
        _ => throw new ArgumentException()
    };

    public ValidateContactHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}