using DomainServices.SalesArrangementService.Api.Repositories.Entities;
using System.Text.Json;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class CreateCustomerHandler
    : IRequestHandler<Dto.CreateCustomerMediatrRequest, _SA.CreateCustomerResponse>
{
    public async Task<_SA.CreateCustomerResponse> Handle(Dto.CreateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        // check existing SalesArrangementId
        await _saRepository.GetSalesArrangement(request.Request.SalesArrangementId, cancellation);

        var entity = new Repositories.Entities.CustomerOnSA
        {
            SalesArrangementId = request.Request.SalesArrangementId,
            CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.Request.CustomerRoleId,
            Identities = request.Request.Customer?.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };
        
        // updatovat entitu udaji z requestu, pripadne dotahnout z CM. Zajistit nove MP ID.
        var result = await _identifyCustomerService.FillEntity(entity, request.Request.Customer, cancellation);

        // ulozit do DB
        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
        int customerId = entity.CustomerOnSAId;

        // obligations
        if (request.Request.Customer?.Obligations is not null && request.Request.Customer.Obligations.Any())
        {
            var obligationEntity = new CustomerOnSAObligations
            {
                CustomerOnSAId = customerId,
                Data = JsonSerializer.Serialize(request.Request.Customer.Obligations!.ToList())
            };
            _dbContext.CustomersObligations.Add(obligationEntity);
            await _dbContext.SaveChangesAsync(cancellation);
        }

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSA), customerId);
        
        var model = new _SA.CreateCustomerResponse()
        {
            CustomerOnSAId = customerId,
            PartnerId = result.PartnerId
        };
        if (result.Identities is not null)
            model.CustomerIdentifiers.AddRange(result.Identities);
        return model;
    }

    private readonly Shared.IdentifyCustomerService _identifyCustomerService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly ILogger<CreateCustomerHandler> _logger;
    
    public CreateCustomerHandler(
        Shared.IdentifyCustomerService identifyCustomerService,
        Repositories.SalesArrangementServiceDbContext dbContext,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<CreateCustomerHandler> logger)
    {
        _identifyCustomerService = identifyCustomerService;
        _dbContext = dbContext;
        _saRepository = saRepository;
        _logger = logger;
    }
}