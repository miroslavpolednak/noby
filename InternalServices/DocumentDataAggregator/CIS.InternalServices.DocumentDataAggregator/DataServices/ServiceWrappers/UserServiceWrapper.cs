using AutoFixture;
using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[ScopedService, SelfService]
internal class UserServiceWrapper : IServiceWrapper
{
    public Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        data.User = new Fixture().Build<User>().With(x => x.Id, input.UserId!.Value).Create();

        return Task.CompletedTask;
    }
}