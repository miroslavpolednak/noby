using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[ScopedService, SelfService]
internal class UserServiceWrapper : IServiceWrapper
{
    public Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}