using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.IndividualPricingProcessChanged;

public class IndividualPricingProcessChangedConsumer : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged> context)
    {
        await Task.Delay(0);
    }
}