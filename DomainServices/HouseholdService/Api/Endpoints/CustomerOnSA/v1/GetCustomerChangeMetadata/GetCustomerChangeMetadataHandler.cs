using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.GetCustomerChangeMetadata;

internal sealed class GetCustomerChangeMetadataHandler(
    IDocumentDataStorage _documentDataStorage,
    CustomerOnSADataMapper _customerMapper,
    HouseholdServiceDbContext _dbContext)
        : IRequestHandler<GetCustomerChangeMetadataRequest, GetCustomerChangeMetadataResponse>
{
    public async Task<GetCustomerChangeMetadataResponse> Handle(GetCustomerChangeMetadataRequest request, CancellationToken cancellationToken)
    {
        var customers = await _dbContext
            .Customers
            .AsNoTracking()
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .Select(t => t.CustomerOnSAId)
            .ToArrayAsync(cancellationToken);

        GetCustomerChangeMetadataResponse response = new();
        var additionalData = await _documentDataStorage.GetList<Database.DocumentDataEntities.CustomerOnSAData, int>(customers!, cancellationToken);

        foreach (var customerOnSAId in customers)
        {
            var data = additionalData.FirstOrDefault(t => t.EntityId == customerOnSAId);

            var (_, customerChangeMetadata) = _customerMapper.MapFromDataToSingle(data?.Data);
            response.CustomersOnSAMetadata.Add(new GetCustomerChangeMetadataResponse.Types.GetCustomerChangeMetadataResponseItem()
            {
                CustomerOnSAId = customerOnSAId,
                CustomerChangeMetadata = customerChangeMetadata ?? new CustomerChangeMetadata()
            });
        }

        return response;
    }
}
