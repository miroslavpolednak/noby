using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Database;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal class LoanRepository
{
    private readonly ProductServiceDbContext _dbContext;

    public LoanRepository(ProductServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Entities.LoanPurpose>> GetPurposes(long loanId, CancellationToken cancellation)
    {
        return await _dbContext.LoanPurposes
            .AsNoTracking()
            .Where(t => t.UverId == loanId)
            .ToListAsync(cancellation);
    }

    public async Task<Entities.Loan> GetLoan(long loanId, CancellationToken cancellation)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _dbContext.Loans
            .AsNoTracking()
            .Where(t => t.Id == loanId)
            .FirstOrDefaultAsync(cancellation);
#pragma warning restore CS8603 // Possible null reference return.
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
            .Where(t => t.UverId == loanId && t.PartnerId == partnerId)
            .AnyAsync(cancellation);
    }

    public async Task<bool> ExistsPartner(long partnerId, CancellationToken cancellation)
    {
        return await _dbContext.Partners
            .AsNoTracking()
            .Where(t => t.Id == partnerId)
            .AnyAsync(cancellation);
    }
}