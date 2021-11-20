using ProtoBuf.Grpc.Client;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("DomainServices.CodebookService.Tests.TestStartup", "DomainServices.CodebookService.Tests")]

namespace DomainServices.CodebookService.Tests;

/// <summary>
/// Toto se spusti jednou na zacatku a na konci test session
/// https://stackoverflow.com/questions/13829737/run-code-once-before-and-after-all-tests-in-xunit-net
/// </summary>
public class TestStartup : XunitTestFramework
{
    public TestStartup(IMessageSink messageSink)
        : base(messageSink)
    {
        // init global settings
        CIS.Testing.GlobalTestsSettings.Init("DomainServices/CodebookService/Api", opt =>
        {
            opt.SetBaseNamespace("DomainServices.CodebookService.Tests");
        });

        GrpcClientFactory.AllowUnencryptedHttp2 = true;
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}