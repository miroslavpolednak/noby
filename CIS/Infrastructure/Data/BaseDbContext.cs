using CIS.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CIS.Infrastructure.Data;

public abstract class BaseDbContext : DbContext
{
    /// <summary>
    /// ID of current user
    /// </summary>
#pragma warning disable CA1707 // Identifiers should not contain underscores
    protected Core.Security.ICurrentUser? _currentUser { get; init; }
#pragma warning restore CA1707 // Identifiers should not contain underscores

    public BaseDbContext(DbContextOptions options, Core.Security.ICurrentUserAccessor userProvider)
        : base(options)
    {
        _currentUser = userProvider.User;
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
                        if (_currentUser is not null)
                        {
                            obj1.CreatedUserId = _currentUser.Id;
                            obj1.CreatedUserName = _currentUser.Name;
                        }
                        obj1.CreatedTime = DateTime.Now;
                    }
                    if (_currentUser is not null && entry.Entity is IModifiedUser obj2)
                    {
                        obj2.ModifiedUserId = _currentUser.Id;
                        obj2.ModifiedUserName = _currentUser.Name;
                    }
                    break;

                case EntityState.Modified:
                    if (_currentUser is not null && entry.Entity is IModifiedUser obj3)
                    {
                        obj3.ModifiedUserId = _currentUser.Id;
                        obj3.ModifiedUserName = _currentUser.Name;
                    }
                    break;
            }
        }
    }
}
