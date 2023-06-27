using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomerChangeMetadata;

internal sealed class GetCustomerChangeMetadataHandler
    : IRequestHandler<GetCustomerChangeMetadataRequest, GetCustomerChangeMetadataResponse>
{
    public async Task<GetCustomerChangeMetadataResponse> Handle(GetCustomerChangeMetadataRequest request, CancellationToken cancellationToken)
    {
        var customers = await _dbContext
            .Customers
            .AsNoTracking()
            .Where(t => t.SalesArrangementId == request.SalesArrangementId && t.ChangeMetadataBin != null)
            .Select(t => new { t.CustomerOnSAId, t.ChangeMetadataBin })
            .ToListAsync(cancellationToken);

        GetCustomerChangeMetadataResponse response = new();
        response.CustomersOnSAMetadata.AddRange(customers.Select(t => new GetCustomerChangeMetadataResponse.Types.GetCustomerChangeMetadataResponseItem
        {
            CustomerOnSAId = t.CustomerOnSAId,
            CustomerChangeMetadata = CustomerChangeMetadata.Parser.ParseFrom(t.ChangeMetadataBin)
        }));

        return response;
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetCustomerChangeMetadataHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
