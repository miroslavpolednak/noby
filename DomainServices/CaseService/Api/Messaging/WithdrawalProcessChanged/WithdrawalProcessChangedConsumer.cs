using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.WithdrawalProcessChanged;

internal sealed class WithdrawalProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged> context)
    {
        await Task.Delay(0);
    }
}