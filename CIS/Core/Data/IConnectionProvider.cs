using System.Data.Common;

namespace CIS.Core.Data;

public interface IConnectionProvider
{
    string ConnectionString { get; }

    DbConnection Create();
}

public interface IConnectionProvider<TRepository> : IConnectionProvider
{
}
