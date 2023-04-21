using DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService.Entities;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService;

internal interface IDocumentArchiveRepository
{
    Task SaveDataSentenseWithForm(DocumentInterface[] documentInterfaces, CancellationToken cancellationToken);
}

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal sealed class DocumentArchiveRepository : IDocumentArchiveRepository
{
    private readonly DocumentArchiveServiceDbContext _dbContext;

    public DocumentArchiveRepository(DocumentArchiveServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveDataSentenseWithForm(DocumentInterface[] documentInterfaces, CancellationToken cancellationToken)
    {
        await _dbContext.DocumentInterface.AddRangeAsync(documentInterfaces, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
