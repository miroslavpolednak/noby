using DomainServices.CustomerService.Clients.v1;
using DomainServices.CustomerService.Contracts;
using NOBY.Api.Endpoints.Customer.SearchCustomers;

namespace NOBY.Api.Endpoints.Customer.Identify;

internal sealed class IdentifyHandler(
    ICustomerServiceClient _customerService, 
    ILogger<IdentifyHandler> _logger)
        : IRequestHandler<CustomerIdentifyRequest, CustomerInList?>
{
    public async Task<CustomerInList?> Handle(CustomerIdentifyRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.CustomerService.Contracts.SearchCustomersRequest
        {
            NaturalPerson = new NaturalPersonSearch
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.DateOfBirth
            },
            IdentificationDocument = new IdentificationDocumentSearch
            {
                IdentificationDocumentTypeId = request.IdentificationDocumentTypeId,
                IssuingCountryId = request.IssuingCountryId,
                Number = request.IdentificationDocumentNumber ?? ""
            },
            Mandant = SharedTypes.GrpcTypes.Mandants.Kb
        };

        // ID klienta
        if (request.Identity is not null && request.Identity.Id > 0)
        {
            dsRequest.Identity = request.Identity;
        }

        // zavolat sluzbu
        var searchResult = await _customerService.SearchCustomers(dsRequest, cancellationToken);

        if (searchResult.Customers.Count == 1)
        {
            var customer = searchResult.Customers.First();

            return new CustomerInList().FillBaseData(customer);
        }

        if (searchResult.Customers.Count == 0)
            return null;

        _logger.LogInformation("More than 1 client found");
        throw new NobyValidationException($"More than 1 client found: {string.Join(", ", searchResult.Customers.Select(t => t.Identities?.FirstOrDefault()?.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture)))}", 409);
    }
}
