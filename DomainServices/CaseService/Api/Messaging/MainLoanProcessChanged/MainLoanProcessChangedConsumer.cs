using DomainServices.CaseService.Api.Database;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MainLoanProcessChanged;

internal sealed class MainLoanProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.MainLoanProcessChanged>
{
    private readonly CaseServiceDbContext _dbContext;

    public MainLoanProcessChangedConsumer(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.MainLoanProcessChanged> context)
    {
        var state = context.Message.state switch
        {
            cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.ProcessStateEnum.TERMINATED => 7,
            cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.ProcessStateEnum.COMPLETED => 6,
            _ => context.Message.processData.@private.mainLoanProcessData.processPhase.code
        };

        if (long.TryParse(context.Message.@case.caseId.id, out long caseId))
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.KafkaMessageIncorrectFormat, context.Message.@case.caseId.id);
        }

        var entity = _dbContext.Cases
            .FirstOrDefault(t => t.CaseId == caseId)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, caseId);

        entity.State = state;

        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }
}
