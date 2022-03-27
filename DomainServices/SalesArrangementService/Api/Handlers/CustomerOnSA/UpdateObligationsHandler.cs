using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateObligationsHandler
    : IRequestHandler<Dto.UpdateObligationsMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateObligationsMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateObligationsHandler), request.Request.CustomerOnSAId);

        // overit ciselniky
        if (request.Request.Obligations is not null)
        {
            foreach (var obligation in request.Request.Obligations)
            {
                // druh zavazku
                if (obligation.ObligationTypeId.HasValue && !(await _codebookService.ObligationTypes(cancellation)).Any(t => t.Id == obligation.ObligationTypeId))
                    throw new CisNotFoundException(1, $"ObligationTypeId {obligation.ObligationTypeId} not found");
                // kontrolovat state?
            }
        }
        
        // entita Customera
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.Request.CustomerOnSAId} does not exist.");

        var obligationEntity = await _dbContext.CustomersObligations.FirstOrDefaultAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation);
        if (obligationEntity is null)
        {
            obligationEntity = new Repositories.Entities.CustomerOnSAObligations
            {
                CustomerOnSAId = request.Request.CustomerOnSAId,
            };
            _dbContext.CustomersObligations.Add(obligationEntity);
        }
        obligationEntity.Obligations = request.Request.Obligations is null ? null : JsonSerializer.Serialize(request.Request.Obligations!.ToList());

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<UpdateObligationsHandler> _logger;

    public UpdateObligationsHandler(
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<UpdateObligationsHandler> logger)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}
