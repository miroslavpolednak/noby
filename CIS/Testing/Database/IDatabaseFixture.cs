using System;

namespace CIS.Testing.Database
{
    public interface IDatabaseFixture : IDisposable
    {
        CIS.Core.Data.IConnectionProvider Provider { get; }
    }
}
