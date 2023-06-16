using DomainServices.CustomerService.ExternalServices.Contacts.V1;
using DomainServices.CustomerService.ExternalServices.Contacts.V1.Contracts;
using ValidateContactResponse = DomainServices.CustomerService.Contracts.ValidateContactResponse;

namespace DomainServices.CustomerService.Api.Endpoints.ValidateContact;

internal sealed class ValidateContactHandler : IRequestHandler<ValidateContactRequest, ValidateContactResponse>
{
    public async Task<ValidateContactResponse> Handle(ValidateContactRequest request, CancellationToken cancellationToken)
    {
        return request.ContactType switch
        {
            ContactType.Phone => await HandlePhone(request, cancellationToken),
            ContactType.Email => await HandleEmail(request, cancellationToken),
            _ => throw new ArgumentException()
        };
    }

    private async Task<ValidateContactResponse> HandlePhone(ValidateContactRequest request, CancellationToken cancellationToken)
    {
        var result = await _contactClient.ValidatePhone(request.Contact, cancellationToken);
            
        return new ValidateContactResponse
        {
            ContactType = ContactType.Phone,
            IsContactValid = result.ValidationResult == ValidateContactResponseValidationResult.VALID
        };
    }

    private async Task<ValidateContactResponse> HandleEmail(ValidateContactRequest request, CancellationToken cancellationToken)
    {
        var result = await _contactClient.ValidateEmail(request.Contact, cancellationToken);
            
        return new ValidateContactResponse
        {
            ContactType = ContactType.Email,
            IsContactValid = result.ValidationResult == ValidateContactResponseValidationResult.VALID
        };
    }
    
    private readonly IContactClient _contactClient;
    
    public ValidateContactHandler(IContactClient contactClient)
    {
        _contactClient = contactClient;
    }
}