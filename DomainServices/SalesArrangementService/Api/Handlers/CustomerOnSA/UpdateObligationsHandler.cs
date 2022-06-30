using Microsoft.EntityFrameworkCore;
using Google.Protobuf;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateObligationsHandler
    : IRequestHandler<Dto.UpdateObligationsMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateObligationsMediatrRequest request, CancellationToken cancellation)
    {
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

        // obligations
        var obligationEntity = await _dbContext.CustomersObligations.FirstOrDefaultAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation);
        if (obligationEntity is null)
        {
            obligationEntity = new Repositories.Entities.CustomerOnSAObligations
            {
                CustomerOnSAId = request.Request.CustomerOnSAId,
            };
            _dbContext.CustomersObligations.Add(obligationEntity);
        }

        if (request.Request.Obligations is null)
        {
            obligationEntity.DataBin = null;
            obligationEntity.Data = null;
        }
        else
        {
            // tohle je tu jen kvuli serializaci do bin. Casem nejak refaktorovat.
            var obligationsCollection = new _SA.ObligationsCollection();
            obligationsCollection.Items.AddRange(request.Request.Obligations);

            obligationEntity.DataBin = obligationsCollection!.ToByteArray();
            obligationEntity.Data = Newtonsoft.Json.JsonConvert.SerializeObject(request.Request.Obligations!.ToList());
        }
        
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    
    public UpdateObligationsHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
