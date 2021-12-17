using CIS.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CIS.Infrastructure.Data;

public abstract class BaseDbContext : DbContext
{
    /// <summary>
    /// ID of current user
    /// </summary>
    protected int? _currentUserId = null;

    public BaseDbContext(DbContextOptions options, Core.Security.ICurrentUserAccessor userProvider)
        : base(options)
    {
        _currentUserId = userProvider.User?.Id;
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
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        builder.Properties<DateOnly?>()
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
                    if (entry.Entity is ICreated)
                    {
                        var obj = (ICreated)entry.Entity;
                        obj.CreatedUserId = _currentUserId.GetValueOrDefault();
                        obj.CreatedTime = DateTime.Now;
                    }
                    if (entry.Entity is IModifiedUserId)
                    {
                        ((IModifiedUserId)entry.Entity).ModifiedUserId = _currentUserId.GetValueOrDefault();
                    }
                    break;

                case EntityState.Modified:
                    if (entry.Entity is IModifiedUserId)
                    {
                        ((IModifiedUserId)entry.Entity).ModifiedUserId = _currentUserId.GetValueOrDefault();
                    }
                    break;
            }
        }
    }
}
