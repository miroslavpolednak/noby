using CIS.Foms.Enums;
using Microsoft.EntityFrameworkCore;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
    : IRequestHandler<DeleteSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteSalesArrangementRequest request, CancellationToken cancellation)
    {
        var saInstance = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        if (!request.HardDelete)
        {
            // kontrola na kategorii
            if ((await _codebookService.SalesArrangementTypes(cancellation)).First(t => t.Id == saInstance.SalesArrangementTypeId).SalesArrangementCategory != 2)
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SATypeNotSupported, saInstance.SalesArrangementSignatureTypeId);

            // kontrola na stav
            if (saInstance.State != (int)SalesArrangementStates.InProgress && saInstance.State != (int)SalesArrangementStates.IsSigned)
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SalesArrangementCantDelete, saInstance.State);
        }

        // smazat SA
        _dbContext.Remove(saInstance);
        
        // smazat parametry
        await _dbContext.SalesArrangementsParameters
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .ExecuteDeleteAsync(cancellation);
        
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public DeleteSalesArrangementHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
