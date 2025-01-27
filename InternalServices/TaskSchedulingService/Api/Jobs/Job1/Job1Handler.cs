﻿using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job1;

internal sealed class Job1Handler(ILogger<Job1Handler> _logger)
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Job1Handler is running {D}", DateTime.Now);
        while (!cancellationToken.IsCancellationRequested) 
        {
            await Task.Delay(1000 * 1, cancellationToken);
            _logger.LogInformation("Hit");
        }
        _logger.LogInformation("Job1Handler finished {D}", DateTime.Now);
    }
}
