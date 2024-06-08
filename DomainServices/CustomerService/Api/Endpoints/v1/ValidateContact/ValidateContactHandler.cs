using DomainServices.CustomerService.ExternalServices.Contacts.V1;
using DomainServices.CustomerService.ExternalServices.Contacts.V1.Contracts;
using ValidateContactResponse = DomainServices.CustomerService.Contracts.ValidateContactResponse;

namespace DomainServices.CustomerService.Api.Endpoints.v1.ValidateContact;

internal sealed class ValidateContactHandler(IContactClient _contactClient) : IRequestHandler<ValidateContactRequest, ValidateContactResponse>
{
    public async Task<ValidateContactResponse> Handle(ValidateContactRequest request, CancellationToken cancellationToken)
    {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
        return request.ContactType switch
        {
            ContactType.Phone => await HandlePhone(request, cancellationToken),
            ContactType.Email => await HandleEmail(request, cancellationToken),
            _ => throw new ArgumentException()
        };
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
    }

    private async Task<ValidateContactResponse> HandlePhone(ValidateContactRequest request, CancellationToken cancellationToken)
    {
        var result = await _contactClient.ValidatePhone(request.Contact, cancellationToken);

        return new ValidateContactResponse
        {
            ContactType = ContactType.Phone,
            IsContactValid = result.ValidationResult == ValidateContactResponseValidationResult.VALID,
            IsMobile = result.AdditionalContactInformations.IsMobile
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
}