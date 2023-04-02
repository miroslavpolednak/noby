using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using NOBY.Api.Endpoints.Customer.Search;
using NOBY.Api.Endpoints.Customer.Search.Dto;

namespace NOBY.Api.Endpoints.Customer.Identify;

internal sealed class IdentifyHandler
    : IRequestHandler<IdentifyRequest, CustomerInList>
{
    public async Task<CustomerInList?> Handle(IdentifyRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new SearchCustomersRequest
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
            Mandant = Mandants.Kb
        };

        // ID klienta
        if (request.Identity is not null && request.Identity.Id > 0)
        {
            dsRequest.Identity = new Identity(request.Identity.Id, request.Identity.Scheme);
        }

        // zavolat sluzbu
        var searchResult = await _customerService.SearchCustomers(dsRequest, cancellationToken);

        if (searchResult.Customers.Count == 1)
        {
            var customer = searchResult.Customers.First();

            return new CustomerInList().FillBaseData(customer).FillIdentification(customer.Identity);
        }

        if (!searchResult.Customers.Any())
            return null;

        _logger.LogInformation("More than 1 client found");
        throw new CisConflictException($"More than 1 client found: {string.Join(", ", searchResult.Customers.Select(t => t.Identity?.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture)))}");
    }

    private readonly ILogger<IdentifyHandler> _logger;
    private readonly ICustomerServiceClient _customerService;

    public IdentifyHandler(ICustomerServiceClient customerService, ILogger<IdentifyHandler> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}
