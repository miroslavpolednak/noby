using Microsoft.EntityFrameworkCore;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateCustomerHandler
    : IRequestHandler<Dto.UpdateCustomerMediatrRequest, _SA.UpdateCustomerResponse>
{
    public async Task<_SA.UpdateCustomerResponse> Handle(Dto.UpdateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == request.Request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.Request.CustomerOnSAId} does not exist.");

        // jestlize uz ma MP identitu, neni co menit - asi vyhod chybu? Nebo ne?
        /*if (entity.Identities is not null && entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Mp))
            throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, "CustomerOnSA already contains Identity", 16033);*/

        var model = new _SA.UpdateCustomerResponse();

        // uz ma KB identitu, ale jeste nema MP identitu
        if (entity.Identities is not null && !entity.Identities.Any(t => t.IdentityScheme == CIS.Foms.Enums.IdentitySchemes.Mp))
        {
            var identity = entity.Identities.First();
            int? newMpId = await _identifyCustomerService.TryCreateMpIdentity(new CIS.Infrastructure.gRPC.CisTypes.Identity(identity.IdentityId, identity.IdentityScheme), cancellation);

            if (newMpId.HasValue)
            {
                // update zakladnich dat
                entity.HasPartner = request.Request.Customer.HasPartner;
                entity.LockedIncomeDateTime = request.Request.Customer.LockedIncomeDateTime;

                // ulozit do DB
                _dbContext.CustomersIdentities.Add(new Repositories.Entities.CustomerOnSAIdentity
                {
                    CustomerOnSAId = request.Request.CustomerOnSAId,
                    IdentityId = newMpId.Value,
                    IdentityScheme = CIS.Foms.Enums.IdentitySchemes.Mp
                });
                await _dbContext.SaveChangesAsync(cancellation);

                // vratit vysledek
                model.PartnerId = newMpId;
                model.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new CIS.Infrastructure.gRPC.CisTypes.Identity(t.IdentityId, t.IdentityScheme)));
                model.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(newMpId.Value, CIS.Foms.Enums.IdentitySchemes.Mp));
            }
            else
                throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, "CustomerOnSA already contains KB Identity and create MP Identity failed", 16054);
        }
        else
        {
            // updatovat entitu udaji z requestu, pripadne dotahnout z CM. Zajistit nove MP ID.
            var result = await _identifyCustomerService.FillEntity(entity, request.Request.Customer, cancellation);

            await _dbContext.SaveChangesAsync(cancellation);

            model.PartnerId = result.PartnerId;
            if (result.Identities is not null)
                model.CustomerIdentifiers.AddRange(result.Identities);
        }

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