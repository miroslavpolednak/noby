using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.SalesArrangementService.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetCustomerHandler
    : IRequestHandler<Dto.GetCustomerMediatrRequest, Contracts.CustomerOnSA>
{
    public async Task<Contracts.CustomerOnSA> Handle(Dto.GetCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomerHandler), request.CustomerOnSAId);
        
        var customerInstance = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceRepositoryExpressions.CustomerDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // identity
        var identities = await _dbContext.CustomersIdentities
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .AsNoTracking()
            .ToListAsync(cancellation);
        customerInstance.CustomerIdentifiers.AddRange(identities.Select(t => new Identity(t.Id, t.IdentityScheme)));

        // obligations
        var obligations = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(t => t.Obligations)
            .FirstOrDefaultAsync(cancellation);
        if (!string.IsNullOrEmpty(obligations))
            customerInstance.Obligations.AddRange(JsonSerializer.Deserialize<List<Contracts.CustomerObligation>>(obligations, GrpcHelpers.GrpcJsonSerializerOptions));

        return customerInstance;
    }
    
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
        private readonly ILogger<GetCustomerHandler> _logger;
    
    public GetCustomerHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<GetCustomerHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}