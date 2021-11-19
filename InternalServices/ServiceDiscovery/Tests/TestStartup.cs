global using Xunit;

using ProtoBuf.Grpc.Client;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("CIS.InternalServices.ServiceDiscovery.Tests.TestStartup", "CIS.InternalServices.ServiceDiscovery.Tests")]

namespace CIS.InternalServices.ServiceDiscovery.Tests;

/// <summary>
/// Toto se spusti jednou na zacatku a na konci test session
/// https://stackoverflow.com/questions/13829737/run-code-once-before-and-after-all-tests-in-xunit-net
/// </summary>
public class TestStartup : XunitTestFramework
{
    public TestStartup(IMessageSink messageSink)
        : base(messageSink)
    {
        // mapovani dapper - Guid
        CIS.Testing.Database.SqliteGuidTypeHandler.Register();

        // init global settings
        Testing.GlobalTestsSettings.Init("InternalServices/ServiceDiscovery/Api", opt =>
        {
            opt.SetBaseNamespace("CIS.InternalServices.ServiceDiscovery.Tests");
        });

        GrpcClientFactory.AllowUnencryptedHttp2 = true;
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}