using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Testing.Database;
public class EfInMemoryMockAdapter : IDbMockAdapter
{
    public void MockDatabase<TStartup>(IServiceCollection services) where TStartup : class
    {
        var dbContextTypes = typeof(TStartup).Assembly.GetTypes().Where(t => typeof(DbContext).IsAssignableFrom(t));
        var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions).GetMethods().Where(i => i.Name == "AddDbContext" && i.IsGenericMethod == true).First();

        foreach (var dbContextType in dbContextTypes)
        {
            // Remove existing dbContext(real db) 
            var dbContextOptions = typeof(DbContextOptions<>).MakeGenericType(dbContextType);
            var dbContextDescriptor = services.SingleOrDefault(
                                    d => d.ServiceType == dbContextOptions);

            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            //Dynamically register db context with in memory db
            var dbName = Guid.NewGuid().ToString(); // db is unique per test class 
            var addDbContextGenericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);
            var optionsAction = new Action<DbContextOptionsBuilder>(options =>
            {
                options.UseInMemoryDatabase(dbName);
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Create AddDbContext parameters
            object[] parametersArray = new object[] { services, optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped };
            // Call AddDbContext with parameters
            addDbContextGenericMethod?.Invoke(services, parametersArray);
        }
    }

    public void Dispose(bool disposing)
    {
        // Nothing to dispose if we use inmemory database
    }
}
