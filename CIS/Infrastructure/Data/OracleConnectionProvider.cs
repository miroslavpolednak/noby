using Oracle.ManagedDataAccess.Client;
using System.Data.Common;


namespace CIS.Infrastructure.Data;

public class OracleConnectionProvider<TConnectionProvider>
    : OracleConnectionProvider, Core.Data.IConnectionProvider<TConnectionProvider>
{
    public OracleConnectionProvider(string connectionString) : base(connectionString)
    { }
}

public class OracleConnectionProvider : CIS.Core.Data.IConnectionProvider
{
    public string ConnectionString { get; init; }

    public OracleConnectionProvider(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public DbConnection Create()
    {
        return new OracleConnection(ConnectionString);
    }
}