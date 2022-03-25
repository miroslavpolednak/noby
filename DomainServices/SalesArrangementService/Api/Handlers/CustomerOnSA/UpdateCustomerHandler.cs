using DomainServices.SalesArrangementService.Api.Repositories.Entities;
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
        int? newMpIdentityId = null;

        // jestlize uz ma nejakou identitu, neni co menit - asi vyhod chybu? Nebo ne?
        if (entity.Identities.Any())
        {

        }

        entity.CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.Request.CustomerRoleId;
        entity.FirstNameNaturalPerson = request.Request.FirstNameNaturalPerson;
        entity.Name = request.Request.Name;
        entity.DateOfBirthNaturalPerson = request.Request.DateOfBirthNaturalPerson;

        if (request.Request.CustomerIdentifiers is not null && request.Request.CustomerIdentifiers.Any())
        {
            entity.Identities = new List<CustomerOnSAIdentity>();
            entity.Identities.AddRange(request.Request.CustomerIdentifiers.Select(t => new CustomerOnSAIdentity(t, request.Request.CustomerOnSAId)));
        }

        await _dbContext.SaveChangesAsync(cancellation);

        return new _SA.UpdateCustomerResponse
        {
            PartnerId = newMpIdentityId
        };
    }
    
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<UpdateCustomerHandler> _logger;
    
    public UpdateCustomerHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<UpdateCustomerHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}