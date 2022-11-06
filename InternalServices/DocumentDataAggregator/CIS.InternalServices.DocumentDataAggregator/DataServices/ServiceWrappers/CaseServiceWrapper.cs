using AutoFixture;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CaseService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CaseServiceWrapper : IServiceWrapper
{
    public Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        var fixture = new Fixture();

        fixture.Customize<GrpcDate>(x => x.FromFactory(() => fixture.Create<DateTime>()).OmitAutoProperties());
        fixture.Customize<NullableGrpcDate>(x => x.FromFactory(() => fixture.Create<DateTime>()!).OmitAutoProperties());

        data.Case = fixture.Build<Case>().Create();

        return Task.CompletedTask;
    }
}