using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Testing.Database;
public static class DbHelper
{
    public static void RemoveExistingDbContext(IServiceCollection services, Type dbContextType)
    {
        var dbContextOptions = typeof(DbContextOptions<>).MakeGenericType(dbContextType);
        var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == dbContextOptions);

        if (dbContextDescriptor is not null)
            services.Remove(dbContextDescriptor);
    }

    public static void RegisterDbContext(IServiceCollection services, System.Reflection.MethodInfo addDbContextGenericMethod, Action<DbContextOptionsBuilder> optionsAction)
    {
        // Create AddDbContext parameters
        object[] parametersArray = new object[] { services, optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped };
        // Call AddDbContext with parameters
        addDbContextGenericMethod?.Invoke(services, parametersArray);
    }
}
