using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MortgageInstanceChanged;

internal sealed class MortgageInstanceChangedConsumer
    : IConsumer<cz.kb.api.mortgageservicingevents.v1.MortgageInstanceChanged>
{
    public async Task Consume(ConsumeContext<cz.kb.api.mortgageservicingevents.v1.MortgageInstanceChanged> context)
    {
        var message = context.Message;
        var cancellationToken = context.CancellationToken;
        //message.New.ApplicationSalesArrangement.salesArrangementId.id

        var instance = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == 1, cancellationToken);
        if (instance is not null)
        {
            instance.TargetAmount = (decimal)message.New.loanAmount.limit;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private readonly Database.CaseServiceDbContext _dbContext;

    public MortgageInstanceChangedConsumer(Database.CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
