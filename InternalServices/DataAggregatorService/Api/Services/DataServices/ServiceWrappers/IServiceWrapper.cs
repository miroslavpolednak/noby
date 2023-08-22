﻿namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

internal interface IServiceWrapper
{
    DataService DataService { get; }

    Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken);
}