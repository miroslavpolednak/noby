global using Xunit;

using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("CIS.Infrastructure.Caching.Tests.TestStartup", "CIS.Infrastructure.Caching.Tests")]

namespace CIS.Infrastructure.Caching.Tests;

public class TestStartup : XunitTestFramework
{
    public TestStartup(IMessageSink messageSink)
        : base(messageSink)
    {
            
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}
