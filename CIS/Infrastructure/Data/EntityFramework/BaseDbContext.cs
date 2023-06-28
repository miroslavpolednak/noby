using CIS.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CIS.Infrastructure.Data;

public abstract class BaseDbContext<TDbContext>
    : DbContext 
    where TDbContext : DbContext
{
    public Core.Security.ICurrentUserAccessor? CurrentUser { get; init; }
    public Core.IDateTime CisDateTime { get; init; }

    public BaseDbContext(BaseDbContextAggregate<TDbContext> aggregate)
        : base(aggregate.Options)
    {
        CurrentUser = aggregate.CurrentUser;
        CisDateTime = aggregate.DateTime;
    }

    /// <summary>
    /// Automaticka aplikace created/modified/actual interfacu
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await addInterfaceFields(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        addInterfaceFields().GetAwaiter().GetResult();
        return base.SaveChanges();
    }

    #region DateOnly conversions
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        configurationBuilder.Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter>()
            .HaveColumnType("date");
    }

    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        public NullableDateOnlyConverter() : base(
            d => d == null
                ? null
                : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
            d => d == null
                ? null
                : new DateOnly?(DateOnly.FromDateTime(d.Value)))
        { }
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }
    #endregion DateOnly conversions

    private async Task addInterfaceFields(CancellationToken cancellationToken = default)
    {
        foreach (var entry in this.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreated obj1 && obj1.CreatedUserId.GetValueOrDefault(0) == 0)
                    {
                        if (CurrentUser!.IsAuthenticated)
                        {
                            await CurrentUser!.EnsureDetails(cancellationToken);
                            obj1.CreatedUserId = CurrentUser!.User!.Id;
                            obj1.CreatedUserName = CurrentUser!.UserDetails!.DisplayName;
                        }
                        obj1.CreatedTime = CisDateTime.Now;
                    }
                    if (CurrentUser is not null && CurrentUser.IsAuthenticated && entry.Entity is IModifiedUser obj2)
                    {
                        if (CurrentUser!.IsAuthenticated)
                        {
                            await CurrentUser!.EnsureDetails(cancellationToken);
                            obj2.ModifiedUserId = CurrentUser!.User!.Id;
                            obj2.ModifiedUserName = CurrentUser!.UserDetails!.DisplayName;
                        }
                    }
                    break;

                case EntityState.Modified:
                    if (CurrentUser is not null && CurrentUser.IsAuthenticated && entry.Entity is IModifiedUser obj3)
                    {
                        if (CurrentUser!.IsAuthenticated)
                        {
                            await CurrentUser!.EnsureDetails(cancellationToken);
                            obj3.ModifiedUserId = CurrentUser!.User!.Id;
                            obj3.ModifiedUserName = CurrentUser!.UserDetails!.DisplayName;
                        }
                    }
                    break;
            }
        }
    }
}
