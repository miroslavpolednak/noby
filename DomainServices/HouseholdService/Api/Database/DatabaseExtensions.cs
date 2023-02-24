using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace DomainServices.HouseholdService.Api.Database;

internal static class DatabaseExtensions
{
    public static async Task<Entities.Household> GetHousehold(this HouseholdServiceDbContext dbContext, int householdId, CancellationToken cancellationToken)
    {
        return await dbContext.Households
            .Where(t => t.HouseholdId == householdId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(16022, $"Household ID {householdId} does not exist.");
    }

    public static async Task<bool> CustomerExistOnSalesArrangement(this HouseholdServiceDbContext dbContext, int customerOnSAId, int salesArrangementId, CancellationToken cancellationToken)
    {
        return await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId && t.SalesArrangementId == salesArrangementId, cancellationToken);
    }
}
