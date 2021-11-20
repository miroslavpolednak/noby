global using Xunit;

using ProtoBuf.Grpc.Client;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("DomainServices.OfferService.Tests.TestStartup", "DomainServices.OfferService.Tests")]

namespace DomainServices.OfferService.Tests;

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
        CIS.Testing.GlobalTestsSettings.Init("../DomainServices/OfferService/Api", opt =>
        {
            opt.SetBaseNamespace("DomainServices.OfferService.Tests");
        });

        GrpcClientFactory.AllowUnencryptedHttp2 = true;
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}