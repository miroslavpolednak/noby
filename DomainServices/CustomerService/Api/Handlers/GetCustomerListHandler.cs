using CIS.Core.Exceptions;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Handlers;

internal class GetCustomerListHandler : IRequestHandler<GetCustomerListMediatrRequest, CustomerListResponse>
{
    private readonly CustomerManagementDetailProvider _cmDetailProvider;
    private readonly KonsDbDetailProvider _konsDbDetailProvider;
    private readonly ILogger<GetCustomerListHandler> _logger;

    public GetCustomerListHandler(CustomerManagementDetailProvider cmDetailProvider, KonsDbDetailProvider konsDbDetailProvider, ILogger<GetCustomerListHandler> logger)
    {
        _cmDetailProvider = cmDetailProvider;
        _konsDbDetailProvider = konsDbDetailProvider;
        _logger = logger;
    }

    public async Task<CustomerListResponse> Handle(GetCustomerListMediatrRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get list instance Identities #{id}", string.Join(",", request.Identities));

        var identitiesLookup = request.Identities.ToLookup(x => x.IdentityScheme, y => y.IdentityId);

        var customers = Enumerable.Concat(await GetCMCustomers(identitiesLookup[Identity.Types.IdentitySchemes.Kb], cancellationToken),
                                          await GetKonsDbCustomers(identitiesLookup[Identity.Types.IdentitySchemes.Mp], cancellationToken));

        var response = new CustomerListResponse();

        response.Customers.AddRange(customers);

        CheckMissingCustomers(response.Customers, request.Identities);

        return response;
    }

    private async Task<IEnumerable<CustomerDetailResponse>> GetCMCustomers(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        if (!customerIds.Any())
            return Enumerable.Empty<CustomerDetailResponse>();

        return await _cmDetailProvider.GetList(customerIds, cancellationToken);
    }

    private async Task<IEnumerable<CustomerDetailResponse>> GetKonsDbCustomers(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        if (!partnerIds.Any())
            return Enumerable.Empty<CustomerDetailResponse>();

        return await _konsDbDetailProvider.GetList(partnerIds, cancellationToken);
    }

    private static void CheckMissingCustomers(ICollection<CustomerDetailResponse> customers, ICollection<Identity> identities)
    {
        if (customers.Count == identities.Count)
            return;

        var missingCustomers = identities.Except(customers.Join(identities,
                                                                x => new { x.Identity.IdentityId, x.Identity.IdentityScheme },
                                                                y => new { y.IdentityId, y.IdentityScheme },
                                                                (_, x) => x)).ToList();

        if (!missingCustomers.Any())
            return;

        var messageIds = string.Join(", ", missingCustomers.Select(c => $"{c.IdentityScheme} - {c.IdentityId}"));
        throw new CisNotFoundException(11000, $"Customers with ID: {messageIds} do not exist.");
    }
}