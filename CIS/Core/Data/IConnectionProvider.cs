using System.Data.Common;

namespace CIS.Core.Data;

/// <summary>
/// Marker interface pro Dapper.
/// </summary>
public interface IConnectionProvider
{
    /// <summary>
    /// Connection string do databáze
    /// </summary>
    string ConnectionString { get; }

    DbConnection Create();
}

/// <summary>
/// Marker interface pro Dapper.
/// </summary>
/// <remarks>
/// TRepository je connection string (ve formě třídy/interface) pro který je daný marker uložený v DI. Existuje proto, aby bylo možné používat v jedné aplikaci více různých connection stringů.
/// </remarks>
public interface IConnectionProvider<TRepository>
    : IConnectionProvider
{
}