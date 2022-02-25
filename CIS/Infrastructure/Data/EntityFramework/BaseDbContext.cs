using CIS.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CIS.Infrastructure.Data;

public abstract class BaseDbContext<TDbContext>
    : DbContext 
    where TDbContext : DbContext
{
    public Core.Security.ICurrentUserAccessor? CurrentUser { get; init; }
    public CIS.Core.IDateTime CisDateTime { get; init; }

    public BaseDbContext(BaseDbContextAggregate<TDbContext> aggregate)
        : base(aggregate.Options)
    {
        CurrentUser = aggregate.CurrentUser;
        CisDateTime = aggregate.DateTime;
    }

    /// <summary>
    /// Automaticka aplikace created/modified/actual interfacu
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        addInterfaceFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        addInterfaceFields();
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

    private void addInterfaceFields()
    {
        foreach (var entry in this.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreated obj1 && obj1.CreatedUserId == 0)
                    {
                        if (CurrentUser is not null)
                        {
                            obj1.CreatedUserId = CurrentUser.User.Id;
                            obj1.CreatedUserName = CurrentUser.User.Name;
                        }
                        obj1.CreatedTime = CisDateTime.Now;
                    }
                    if (CurrentUser is not null && entry.Entity is IModifiedUser obj2)
                    {
                        obj2.ModifiedUserId = CurrentUser.User.Id;
                        obj2.ModifiedUserName = CurrentUser.User.Name;
                    }
                    break;

                case EntityState.Modified:
                    if (CurrentUser is not null && entry.Entity is IModifiedUser obj3)
                    {
                        obj3.ModifiedUserId = CurrentUser.User.Id;
                        obj3.ModifiedUserName = CurrentUser.User.Name;
                    }
                    break;
            }
        }
    }
}
