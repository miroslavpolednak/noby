using DomainServices.CustomerService.ExternalServices.Contacts.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

internal sealed class MockContactClient : IContactClient
{
    public Task<ValidateContactResponse> ValidatePhone(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ValidateContactResponse
        {
            ValidationResult = ValidateContactResponseValidationResult.VALID
        });
    }

    public Task<ValidateContactResponse> ValidateEmail(string emailAddress, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ValidateContactResponse
        {
            ValidationResult = ValidateContactResponseValidationResult.VALID
        });
    }
}