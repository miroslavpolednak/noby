using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;
using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerListHandler : IRequestHandler<GetCustomerListMediatrRequest, CustomerListResponse>
    {
        private readonly CustomerManagementListProvider _cmListProvider;
        private readonly KonsDbListProvider _konsDbListProvider;
        private readonly ILogger<GetCustomerListHandler> _logger;

        public GetCustomerListHandler(CustomerManagementListProvider cmListProvider, KonsDbListProvider konsDbListProvider, ILogger<GetCustomerListHandler> logger)
        {
            _cmListProvider = cmListProvider;
            _konsDbListProvider = konsDbListProvider;
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

        private async Task<IEnumerable<CustomerListItem>> GetCMCustomers(IEnumerable<long> customerIds, CancellationToken cancellationToken)
        {
            if (!customerIds.Any())
                return Enumerable.Empty<CustomerListItem>();

            return await _cmListProvider.GetList(customerIds, cancellationToken);
        }

        private async Task<IEnumerable<CustomerListItem>> GetKonsDbCustomers(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
        {
            if (!partnerIds.Any())
                return Enumerable.Empty<CustomerListItem>();

            return await _konsDbListProvider.GetList(partnerIds, cancellationToken);
        }

        private static void CheckMissingCustomers(ICollection<CustomerListItem> customers, ICollection<Identity> identities)
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
}
