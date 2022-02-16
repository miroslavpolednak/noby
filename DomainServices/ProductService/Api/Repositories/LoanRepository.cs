using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.ProductService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class LoanRepository
{
    private readonly ProductServiceDbContext _dbContext;

    public LoanRepository(ProductServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Entities.Loan> GetLoan(long loanId, CancellationToken cancellation)
    {
        return await _dbContext.Loans
            .AsNoTracking()
            .Where(t => t.Id == loanId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Loan #{loanId} not found"); //TODO: error code
    }

    public async Task<bool> ExistsLoan(long loanId, CancellationToken cancellation)
    {
        return await _dbContext.Loans
            .AsNoTracking()
            .Where(t => t.Id == loanId)
            .AnyAsync(cancellation);
    }

    public async Task<List<Entities.Relationship>> GetRelationships(long loanId, CancellationToken cancellation)
    {
        return await _dbContext.Relationships
            .AsNoTracking()
            .Where(t => t.UverId == loanId)
            .ToListAsync(cancellation);
    }

    public async Task<bool> ExistsRelationship(long loanId, long partnerId, CancellationToken cancellation)
    {
        return await _dbContext.Relationships
            .AsNoTracking()
            .Where(t => t.Id == loanId && t.PartnerId == partnerId)
            .AnyAsync(cancellation);
    }
}