using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SharedComponents.DocumentDataStorage.Database;

internal sealed class DocumentDataDbContext
    : BaseDbContext<DocumentDataDbContext>
{
    public DocumentDataDbContext(BaseDbContextAggregate<DocumentDataDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<DocumentDataStorage> DocumentsData { get; set; }
}
