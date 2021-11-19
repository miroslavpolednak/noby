global using Xunit;

using ProtoBuf.Grpc.Client;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("CIS.InternalServices.Storage.Tests.TestStartup", "CIS.InternalServices.Storage.Tests")]

namespace CIS.InternalServices.Storage.Tests;

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
        CIS.Testing.GlobalTestsSettings.Init("CIS/InternalServices/Storage/Api", opt =>
        {
            opt.SetBaseNamespace("CIS.InternalServices.Storage.Tests");
        });

        GrpcClientFactory.AllowUnencryptedHttp2 = true;
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}
