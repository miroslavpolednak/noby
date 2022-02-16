using CIS.Core.Attributes;
using CIS.Core.Data;
using CIS.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CIS.Infrastructure.Data;

public abstract class BaseDbContext : DbContext
{
    /// <summary>
    /// ID of current user
    /// </summary>
    protected Core.Security.ICurrentUser? CurrentUser { get; init; }
    private readonly CIS.Core.IDateTime _dateTime;

    public BaseDbContext(DbContextOptions options, Core.Security.ICurrentUserAccessor userProvider, CIS.Core.IDateTime dateTime)
        : base(options)
    {
        _dateTime = dateTime;
        CurrentUser = userProvider.User;
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
                            obj1.CreatedUserId = CurrentUser.Id;
                            obj1.CreatedUserName = CurrentUser.Name;
                        }
                        obj1.CreatedTime = _dateTime.Now;
                    }
                    if (CurrentUser is not null && entry.Entity is IModifiedUser obj2)
                    {
                        obj2.ModifiedUserId = CurrentUser.Id;
                        obj2.ModifiedUserName = CurrentUser.Name;
                    }
                    break;

                case EntityState.Modified:
                    if (CurrentUser is not null && entry.Entity is IModifiedUser obj3)
                    {
                        obj3.ModifiedUserId = CurrentUser.Id;
                        obj3.ModifiedUserName = CurrentUser.Name;
                    }
                    break;
            }
        }
    }
}
