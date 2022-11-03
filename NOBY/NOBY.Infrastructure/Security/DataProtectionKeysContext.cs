using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NOBY.Infrastructure.Security;

public sealed class DataProtectionKeysContext
     : DbContext, IDataProtectionKeyContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DataProtectionKeysContext(DbContextOptions<DataProtectionKeysContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options) { }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
}
