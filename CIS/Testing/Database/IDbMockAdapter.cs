using Microsoft.Extensions.DependencyInjection;

namespace CIS.Testing.Database;

public interface IDbMockAdapter
{
    void MockDatabase<TStartup>(IServiceCollection services) where TStartup : class;

    void Dispose(bool disposing);
}
