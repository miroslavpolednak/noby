using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.Services;

internal sealed class HouseholdService : IHouseholdServiceClient
{
    public async Task<int> CreateHousehold(CreateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateHouseholdAsync(request, cancellationToken: cancellationToken);
        return result.HouseholdId;
    }

    public async Task DeleteHousehold(int householdId, bool hardDelete = false, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.DeleteHouseholdAsync(
            new()
            {
                HouseholdId = householdId,
                HardDelete = hardDelete
            }, cancellationToken: cancellationToken);
    }

    public async Task<Household> GetHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetHouseholdAsync(
            new()
            {
                HouseholdId = householdId,
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<Household>> GetHouseholdList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetHouseholdListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return result.Households.ToList();
    }

    public async Task UpdateHousehold(UpdateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateHouseholdAsync(request, cancellationToken: cancellationToken);
    }

    public async Task LinkCustomerOnSAToHousehold(int householdId, int? customerOnSAId1, int? customerOnSAId2, CancellationToken cancellationToken = default(CancellationToken))
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
        if (_cacheValidateHouseholdId is null)
        {
            _cacheValidateHouseholdId = await _service.ValidateHouseholdIdAsync(new ValidateHouseholdIdRequest
            {
                HouseholdId = householdId,
                ThrowExceptionIfNotFound = throwExceptionIfNotFound
            }, cancellationToken: cancellationToken);
        }
        return _cacheValidateHouseholdId;
    }

    private ValidateHouseholdIdResponse? _cacheValidateHouseholdId;

    private readonly Contracts.v1.HouseholdService.HouseholdServiceClient _service;

    public HouseholdService(Contracts.v1.HouseholdService.HouseholdServiceClient service) => _service = service;
}