using Microsoft.Data.Sqlite;
using CIS.Core.Data;
using System.Data.Common;

namespace CIS.Testing.Database
{
    public class SqliteConnectionProvider : IConnectionProvider
    {
        public string ConnectionString { get; init; }

        public SqliteConnectionProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public DbConnection Create()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}
