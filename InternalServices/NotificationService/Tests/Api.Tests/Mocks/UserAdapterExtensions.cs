using AutoFixture;
using CIS.Core.Security;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.User;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using Microsoft.Extensions.Options;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Mocks;

public static class UserAdapterExtensions
{
    public static void MockUserAdapterService(this Fixture fixture, string username)
    {
        var mockAppConfiguration = fixture.Freeze<Mock<IOptions<AppConfiguration>>>();
        
        var mockServiceUser = fixture.Freeze<Mock<IServiceUser>>();
        mockServiceUser
            .Setup(m => m.Name)
            .Returns(username);
        
        var mockServiceUserAccessor = fixture.Freeze<Mock<IServiceUserAccessor>>();
        mockServiceUserAccessor
            .Setup(m => m.User)
            .Returns(mockServiceUser.Object);
        
        fixture.Register<IUserAdapterService>(() => new UserAdapterService(mockServiceUserAccessor.Object, mockAppConfiguration.Object));
    }
}