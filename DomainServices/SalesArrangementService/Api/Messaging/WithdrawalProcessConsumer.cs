using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using MassTransit;

namespace DomainServices.SalesArrangementService.Api.Messaging;

public class WithdrawalProcessConsumer : IConsumer<WithdrawalProcessChanged>
{
    public async Task Consume(ConsumeContext<WithdrawalProcessChanged> context)
    {
        await Task.Delay(0, context.CancellationToken);
    }
}