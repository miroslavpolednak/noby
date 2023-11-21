﻿using SharedTypes.GrpcTypes;
using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomerList;

internal sealed class GetCustomerListHandler
    : IRequestHandler<GetCustomerListRequest, GetCustomerListResponse>
{
    public async Task<GetCustomerListResponse> Handle(GetCustomerListRequest request, CancellationToken cancellationToken)
    {
        var customers = await _dbContext.Customers
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceExpressions.CustomerDetail())
            .ToListAsync(cancellationToken);

        if (customers.Count == 0)
        {
            await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        }

        var ids = customers.Select(t => t.CustomerOnSAId).ToList();

        var identities = await _dbContext.CustomersIdentities
            .Where(t => ids.Contains(t.CustomerOnSAId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        customers.ForEach(t =>
        {
            t.CustomerIdentifiers.AddRange(
                identities
                    .Where(x => x.CustomerOnSAId == t.CustomerOnSAId)
                    .Select(x => new Identity(x.IdentityId, x.IdentityScheme))
            );
        });

        var model = new GetCustomerListResponse();
        model.Customers.AddRange(customers);

        return model;
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly HouseholdServiceDbContext _dbContext;

    public GetCustomerListHandler(HouseholdServiceDbContext dbContext, SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _dbContext = dbContext;
        _salesArrangementService = salesArrangementService;
    }
}