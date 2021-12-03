using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Repositories;

internal sealed class OfferServiceDbContext : CIS.Infrastructure.Data.BaseDbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public OfferServiceDbContext(DbContextOptions<OfferServiceDbContext> options, CIS.Core.Security.ICurrentUserAccessor userProvider)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options, userProvider) { }

    public DbSet<Entities.OfferInstance> OfferModelations { get; set; }
}
