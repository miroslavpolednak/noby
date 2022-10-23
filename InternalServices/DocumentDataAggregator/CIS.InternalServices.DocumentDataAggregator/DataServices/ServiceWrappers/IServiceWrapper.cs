﻿using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

internal interface IServiceWrapper
{
    Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken);
}