using AutoFixture;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Mocks;

public static class ServiceProviderExtensions
{
    public static void MockServiceProvider(this Fixture fixture)
    {
        var mockRepository = fixture.Freeze<Mock<INotificationRepository>>();
        var serviceScope = fixture.Freeze<Mock<IServiceScope>>();
        var serviceScopeFactory = fixture.Freeze<Mock<IServiceScopeFactory>>();
        var mockServiceProvider = fixture.Freeze<Mock<IServiceProvider>>();
        
        serviceScope
            .Setup(x => x.ServiceProvider)
            .Returns(mockServiceProvider.Object);
        
        serviceScopeFactory
            .Setup(x => x.CreateScope())
            .Returns(serviceScope.Object);

        mockServiceProvider
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactory.Object);
        
        mockServiceProvider
            .Setup(x => x.GetService(typeof(INotificationRepository)))
            .Returns(mockRepository.Object);
    }
}