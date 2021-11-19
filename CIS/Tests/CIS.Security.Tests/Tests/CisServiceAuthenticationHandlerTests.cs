using CIS.Security.InternalServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Xunit;

namespace CIS.Security.Tests;

public class CisServiceAuthenticationHandlerTests
{
    public CisServiceAuthenticationHandlerTests()
    {
        CisServiceAuthenticationHandler.ClearLoginsCache();
    }

    [Fact]
    public async Task CorrectAuthentication_ShouldPass()
    {
        // arrange
        string login = "a";
        var ctr = createDefaultCtrObjects();
        var sut = new CisServiceAuthenticationHandler(ctr.Options, ctr.Logger, ctr.Encoder, ctr.Clock, createRealAuthHeaderParser(), createAdValidator(true, login));

        // act
        var result = await sut.HandleAuthenticateInternalAsync("Basic YTph");

        // assert
        Assert.True(result.Succeeded);
        Assert.Equal(login, result?.Principal?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value);
    }

    [Fact]
    public async Task FailedAdValidation_ShouldFail()
    {
        // arrange
        var ctr = createDefaultCtrObjects();
        var sut = new CisServiceAuthenticationHandler(ctr.Options, ctr.Logger, ctr.Encoder, ctr.Clock, createRealAuthHeaderParser(), createAdValidator(false));

        // act
        var result = await sut.HandleAuthenticateInternalAsync("Basic YTph");

        // assert
        Assert.False(result.Succeeded);
        Assert.Equal("Login or password incorrect", result?.Failure?.Message);
    }

    [Fact]
    public async Task FailedHeaderParsing_ShouldFail()
    {
        var ctr = createDefaultCtrObjects();
        var sut = new CisServiceAuthenticationHandler(ctr.Options, ctr.Logger, ctr.Encoder, ctr.Clock, createRealAuthHeaderParser(), createAdValidator(true));

        // act
        var result = await sut.HandleAuthenticateInternalAsync("xxxxxxxxx");

        // assert
        Assert.False(result.Succeeded);
    }

    private ILoginValidator createAdValidator(bool expectedResult, string? login = null)
    {
        var adLogin = new Mock<ILoginValidator>();
        adLogin.Setup(t => t.Validate(login ?? It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => Task.FromResult(expectedResult));
        return adLogin.Object;
    }

    private IAuthHeaderParser createRealAuthHeaderParser()
        => new AuthHeaderParser((new NullLoggerFactory()).CreateLogger<IAuthHeaderParser>());

    private (IOptionsMonitor<CisServiceAuthenticationOptions> Options, ILoggerFactory Logger, UrlEncoder Encoder, ISystemClock Clock) createDefaultCtrObjects()
    {
        var logger = new NullLoggerFactory();
        var options = Mock.Of<IOptionsMonitor<CisServiceAuthenticationOptions>>();
        var enc = Mock.Of<System.Text.Encodings.Web.UrlEncoder>();
        var clock = Mock.Of<ISystemClock>();
            
        return (options, logger, enc, clock);
    }
}
