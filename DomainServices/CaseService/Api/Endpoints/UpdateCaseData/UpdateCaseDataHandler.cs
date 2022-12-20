using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseData;

internal sealed class UpdateCaseDataHandler
    : IRequestHandler<UpdateCaseDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseDataRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw new CisNotFoundException(13000, "Case", request.CaseId);

        // zkontrolovat ProdInstType
        if (!(await _codebookService.ProductTypes(cancellation)).Any(t => t.Id == request.Data.ProductTypeId))
            throw new CisNotFoundException(13014, nameof(request.Data.ProductTypeId), request.Data.ProductTypeId);

        // ulozit do DB
        entity.ContractNumber = request.Data.ContractNumber;
        entity.TargetAmount = request.Data.TargetAmount;
        entity.ProductTypeId = request.Data.ProductTypeId;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly CaseServiceDbContext _dbContext;

    public UpdateCaseDataHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        CaseServiceDbContext dbContext)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
