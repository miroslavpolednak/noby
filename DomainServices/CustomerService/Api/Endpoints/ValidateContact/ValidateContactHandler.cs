namespace DomainServices.CustomerService.Api.Endpoints.ValidateContact;

internal sealed class ValidateContactHandler : IRequestHandler<ValidateContactRequest, ValidateContactResponse>
{
    public async Task<ValidateContactResponse> Handle(ValidateContactRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return new ValidateContactResponse
        {
            ContactType = ContactType.Phone,
            IsContactValid = true
        };
    }
}