using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.Data;

public static class DataExtensions
{
    public static void RegisterCisTemporalTable<TEntity>(this ModelBuilder modelBuilder) where TEntity : class
    {
        modelBuilder
            .Entity<TEntity>()
            .ToTable(b => b.IsTemporal(x =>
            {
                x.HasPeriodStart("ValidFrom").HasColumnName("ValidFrom");
                x.HasPeriodEnd("ValidTo").HasColumnName("ValidTo");
            }));
    }
}
