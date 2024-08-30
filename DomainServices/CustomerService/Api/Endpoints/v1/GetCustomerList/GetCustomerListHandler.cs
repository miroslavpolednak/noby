using DomainServices.CustomerService.Api.Services;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using ExternalServices.MpHome.V1;

namespace DomainServices.CustomerService.Api.Endpoints.v1.GetCustomerList;

internal sealed class GetCustomerListHandler(
    CustomerManagementDetailProvider _cmDetailProvider,
    IMpHomeClient _mpHome,
    MpHomeDetailMapper _mpHomeDetailMapper)
    : IRequestHandler<CustomerListRequest, CustomerListResponse>
{
    public async Task<CustomerListResponse> Handle(CustomerListRequest request, CancellationToken cancellationToken)
    {
        var identitiesLookup = request.Identities.ToLookup(x => x.IdentityScheme, y => y.IdentityId);

        var customers = (await GetCMCustomers(identitiesLookup[SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb], cancellationToken)).Concat(await GetKonsDbCustomers(identitiesLookup[SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp], cancellationToken));

        CheckMissingCustomers(customers, request.Identities);

        return new CustomerListResponse
        {
            Customers = { customers }
        };
    }

    private async Task<IEnumerable<Customer>> GetCMCustomers(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        if (!customerIds.Any())
        {
            return [];
        }

        return await _cmDetailProvider.GetList(customerIds, cancellationToken).ToListAsync(cancellationToken);
    }

    private async Task<IEnumerable<Customer>> GetKonsDbCustomers(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        if (!partnerIds.Any())
        {
            return [];
        }

        //!!! docasne nez MpHome udela search podle idcek
        List<Customer> list = new();
        foreach (var customer in partnerIds)
        {
            var detail = await _mpHome.GetPartner(customer, cancellationToken)
                ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerNotFound, customer);

            list.Add(await _mpHomeDetailMapper.MapDetailResponse(detail, cancellationToken));
        }
        return list;
    }

    private static void CheckMissingCustomers(IEnumerable<Customer> customers, ICollection<SharedTypes.GrpcTypes.Identity> identities)
    {
        if (customers.Count() == identities.Count)
            return;

        var missingCustomers = identities.Except(customers.SelectMany(c => c.Identities)
                                                          .Join(identities,
                                                                x => new { x.IdentityId, x.IdentityScheme },
                                                                y => new { y.IdentityId, y.IdentityScheme },
                                                                (_, x) => x)).ToList();

        if (missingCustomers.Count == 0)
            return;

        var messageIds = string.Join(", ", missingCustomers.Select(c => $"{c.IdentityScheme} - {c.IdentityId}"));
        throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerNotFound, messageIds);
    }
}