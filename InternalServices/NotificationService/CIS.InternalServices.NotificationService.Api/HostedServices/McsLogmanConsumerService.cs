using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.HostedServices;

public class McsLogmanConsumerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<McsLogmanConsumerService> _logger;

    public McsLogmanConsumerService(
        IServiceProvider serviceProvider,
        ILogger<McsLogmanConsumerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mcsLogmanService = scope.ServiceProvider.GetRequiredService<McsLogmanService>();
        
        await mcsLogmanService.ConsumeResult(stoppingToken, async result =>
        {
            var notificationReport = result.Message.Value;
            if (!Guid.TryParse(notificationReport.id, out var notificationId))
            {
                _logger.LogError("Could not parse notificationId: {id}", notificationReport.id);
            }

            using var innerScope = _serviceProvider.CreateScope();
            var repository = innerScope.ServiceProvider.GetRequiredService<NotificationRepository>();
            
            await repository.UpdateResult(
                notificationId,
                NotificationState.Delivered,
                new HashSet<string>(),
                stoppingToken);
        });
    }
}