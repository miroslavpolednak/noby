﻿using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.OfferService.Clients.Interfaces;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.DeleteRefixationOfferOlderThan;

public class DeleteRefixationOfferOlderThanHandler(IMaintananceServiceClient maintanance)
    : IJob
{
    private readonly IMaintananceServiceClient _maintanance = maintanance;

    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        await _maintanance.DeleteRefixationOfferOlderThan(new(), cancellationToken);
    }
}
