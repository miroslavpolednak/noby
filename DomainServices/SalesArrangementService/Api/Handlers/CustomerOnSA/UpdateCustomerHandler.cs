using Microsoft.EntityFrameworkCore;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateCustomerHandler
    : IRequestHandler<Dto.UpdateCustomerMediatrRequest, _SA.UpdateCustomerResponse>
{
    public async Task<_SA.UpdateCustomerResponse> Handle(Dto.UpdateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(UpdateCustomerHandler));

        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.Request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.Request.CustomerOnSAId} does not exist.");

        //entity.Identities.Add(new Repositories.Entities.CustomerOnSAIdentity(new CIS.Infrastructure.gRPC.CisTypes.Identity(111, CIS.Foms.Enums.IdentitySchemes.Mp), 1));
        await _dbContext.SaveChangesAsync(cancellation);

        // jestlize uz ma nejakou identitu, neni co menit - asi vyhod chybu? Nebo ne?
        if (entity.Identities is not null && entity.Identities.Any())
            throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, "CustomerOnSA already contains Identity", 0);

        // updatovat entitu udaji z requestu, pripadne dotahnout z CM. Zajistit nove MP ID.
        var result = await _identifyCustomerService.FillEntity(entity, request.Request.Customer, cancellation);

        await _dbContext.SaveChangesAsync(cancellation);

        var model = new _SA.UpdateCustomerResponse
        {
            PartnerId = result.PartnerId
        };
        if (result.Identities is not null)
            model.CustomerIdentifiers.AddRange(result.Identities);
        return model;
    }

    private readonly Shared.IdentifyCustomerService _identifyCustomerService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<UpdateCustomerHandler> _logger;
    
    public UpdateCustomerHandler(
        Shared.IdentifyCustomerService identifyCustomerService,
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<UpdateCustomerHandler> logger)
    {
        _identifyCustomerService = identifyCustomerService;
        _dbContext = dbContext;
        _logger = logger;
    }
}