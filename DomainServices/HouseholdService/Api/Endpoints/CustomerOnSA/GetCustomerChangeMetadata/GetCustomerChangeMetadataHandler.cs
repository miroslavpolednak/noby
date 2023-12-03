using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomerChangeMetadata;

internal sealed class GetCustomerChangeMetadataHandler
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

        var additionalData = await _documentDataStorage.GetList<Database.DocumentDataEntities.CustomerOnSAData>(customers!, cancellationToken);
        var convertedData = additionalData.Select(t =>
        {
            var (_, customerChangeMetadata) = _customerMapper.MapFromDataToSingle(t?.Data);
            return new GetCustomerChangeMetadataResponse.Types.GetCustomerChangeMetadataResponseItem
            {
                CustomerOnSAId = t.EntityIdInt,
                CustomerChangeMetadata = customerChangeMetadata
            };
        });
        
        GetCustomerChangeMetadataResponse response = new();
        response.CustomersOnSAMetadata.AddRange(convertedData);

        return response;
    }

    private readonly HouseholdServiceDbContext _dbContext;
    private readonly CustomerOnSADataMapper _customerMapper;
    private readonly IDocumentDataStorage _documentDataStorage;

    public GetCustomerChangeMetadataHandler(
        IDocumentDataStorage documentDataStorage, 
        CustomerOnSADataMapper customerMapper,
        HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
        _customerMapper = customerMapper;
    }
}
