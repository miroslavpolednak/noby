using CIS.Infrastructure.Caching.Grpc;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.v1;

internal sealed class HouseholdServiceClient(
    IGrpcClientResponseCache<HouseholdServiceClient> _cache,
    Contracts.v1.HouseholdService.HouseholdServiceClient _service)
        : IHouseholdServiceClient
{
    public async Task<int> CreateHousehold(CreateHouseholdRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateHouseholdAsync(request, cancellationToken: cancellationToken);
        return result.HouseholdId;
    }

    public async Task DeleteHousehold(int householdId, bool hardDelete = false, CancellationToken cancellationToken = default)
    {
        await _service.DeleteHouseholdAsync(
            new()
            {
                HouseholdId = householdId,
                HardDelete = hardDelete
            }, cancellationToken: cancellationToken);
    }

    public async Task<Household> GetHousehold(int householdId, CancellationToken cancellationToken = default)
    {
        return await _service.GetHouseholdAsync(
            new()
            {
                HouseholdId = householdId,
            }, cancellationToken: cancellationToken);
    }

    public async Task<int?> GetHouseholdIdByCustomerOnSAId(int customerOnSAId, CancellationToken cancellationToken = default)
    {
        return (await _service.GetHouseholdIdByCustomerOnSAIdAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId,
            }, cancellationToken: cancellationToken))
            .HouseholdId;
    }

    public async Task<List<Household>> GetHouseholdList(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            salesArrangementId,
            async (c) => await GetHouseholdListWithoutCache(salesArrangementId, c),
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<List<Household>> GetHouseholdListWithoutCache(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return (await _service.GetHouseholdListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken))
            .Households
            .ToList();
    }

    public async Task UpdateHousehold(UpdateHouseholdRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateHouseholdAsync(request, cancellationToken: cancellationToken);
    }

    public async Task LinkCustomerOnSAToHousehold(int householdId, int? customerOnSAId1, int? customerOnSAId2, CancellationToken cancellationToken = default)
    {
        await _service.LinkCustomerOnSAToHouseholdAsync(new LinkCustomerOnSAToHouseholdRequest
        {
            HouseholdId = householdId,
            CustomerOnSAId1 = customerOnSAId1,
            CustomerOnSAId2 = customerOnSAId2,
        }, cancellationToken: cancellationToken);
    }

    public async Task<ValidateHouseholdIdResponse> ValidateHouseholdId(int householdId, bool throwExceptionIfNotFound, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            householdId,
            async (c) => await ValidateHouseholdIdWithoutCache(householdId, throwExceptionIfNotFound, c),
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<ValidateHouseholdIdResponse> ValidateHouseholdIdWithoutCache(int householdId, bool throwExceptionIfNotFound, CancellationToken cancellationToken = default)
    {
        return await _service.ValidateHouseholdIdAsync(new ValidateHouseholdIdRequest
        {
            HouseholdId = householdId,
            ThrowExceptionIfNotFound = throwExceptionIfNotFound
        }, cancellationToken: cancellationToken);
    }
}