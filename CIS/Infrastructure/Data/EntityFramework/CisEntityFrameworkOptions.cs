using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.Data;

public sealed class CisEntityFrameworkOptions<TDbContext> 
    where TDbContext : DbContext
{
}