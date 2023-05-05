using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace CIS.Testing.Database;
public class SqliteInMemoryMockAdapter : IDbMockAdapter, IDisposable
{
    private DbConnection _connection = null!;

    public void MockDatabase<TStartup>(IServiceCollection services) where TStartup : class
    {
        var dbContextTypes = typeof(TStartup).Assembly.GetTypes().Where(t => typeof(DbContext).IsAssignableFrom(t));
        var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions).GetMethods().Where(i => i.Name == "AddDbContext" && i.IsGenericMethod == true).First();

        foreach (var dbContextType in dbContextTypes)
        {
            //Set IsSqlite to true if exist 
            var isSqlServerInfo = dbContextType.GetProperty("IsSqlite");
            if (isSqlServerInfo is not null)
                isSqlServerInfo.SetValue(null, true);

            // Remove existing dbContext(real db) 
            DbHelper.RemoveExistingDbContext(services, dbContextType);

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var addDbContextGenericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);
            var optionsAction = new Action<DbContextOptionsBuilder>(options =>
            {
                options.UseSqlite(_connection);
            });

            DbHelper.RegisterDbContext(services, addDbContextGenericMethod, optionsAction);
        }
    }

    public void Dispose(bool disposing)
    {
        if (disposing)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        if (_connection is not null)
        {
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
