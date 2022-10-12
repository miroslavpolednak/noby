using CIS.Foms.Enums;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal sealed class DeleteSalesArrangementHandler
    : IRequestHandler<Dto.DeleteSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        var saInstance = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");

        // kontrola na kategorii
        if ((await _codebookService.SalesArrangementTypes(cancellation)).First(t => t.Id == saInstance.SalesArrangementTypeId).SalesArrangementCategory != 2)
            throw new CisValidationException(18013, $"SalesArrangement type not supported");

        // kontrola na stav
        if (saInstance.State != (int)SalesArrangementStates.InProgress && saInstance.State != (int)SalesArrangementStates.IsSigned)
            throw new CisValidationException($"SalesArrangement cannot be updated/deleted in this state {saInstance.State}");

        // smazat SA
        _dbContext.Remove(saInstance);
        //TODO predelat v .NET7 na EF nativni delete
        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.SalesArrangementParameters WHERE SalesArrangementId={request.SalesArrangementId}", cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<UpdateSalesArrangementDataHandler> _logger;

    public DeleteSalesArrangementHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceDbContext dbContext,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<UpdateSalesArrangementDataHandler> logger)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
        _repository = repository;
        _logger = logger;
    }
}
