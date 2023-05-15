using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace CIS.Infrastructure.Data;

public class SqlConnectionProvider<TConnectionProvider> 
    : SqlConnectionProvider, Core.Data.IConnectionProvider<TConnectionProvider>
{
    public SqlConnectionProvider(string connectionString) : base(connectionString)
    { }
}

public class SqlConnectionProvider : CIS.Core.Data.IConnectionProvider
{
    public string ConnectionString { get; init; }

    public SqlConnectionProvider(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    public DbConnection Create()
    {
        return new SqlConnection(ConnectionString);
    }
}
