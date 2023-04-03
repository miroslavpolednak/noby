using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Messaging;
using DomainServices.CaseService.Contracts;
using MassTransit;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

internal sealed class GetCaseDetailHandler
    : IRequestHandler<GetCaseDetailRequest, Case>
{
    /// <summary>
    /// Vraci detail Case-u
    /// </summary>
    public async Task<Case> Handle(GetCaseDetailRequest request, CancellationToken cancellation)
    {
        var message = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.MainLoanProcessChanged
        {
            state = cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.ProcessStateEnum.COMPLETED,
            eventId = "a",
            currentTask = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.CurrentTask
            {
                id = "b",
                name = "c",
                type = 1
            },
            @case = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.Case
            {
                caseId = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.CaseId
                {
                    id = "1"
                },
                mortgageInstance = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.MortgageInstance
                {
                    Starbuild = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.StarbuildInstanceId
                    {
                        id = "a"
                    }
                }
            },
            type = 1,
            occurredOn = DateTime.Now,
            id = Guid.NewGuid().ToString("N"),
            name = "d",
            processData = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.ProcessData
            {
                @private = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.BusinessObject
                {
                    mainLoanProcessData = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.MainLoanProcessData
                    {
                        processPhase = new cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.ProcessPhase
                        {
                            code = 1,
                            name = "a"
                        }
                    }
                }
            }
        };

        await _producer.Produce(message, cancellation);

        return new Case();

        // vytahnout Case z DB
        return await _dbContext.Cases
            .Where(t => t.CaseId == request.CaseId)
            .AsNoTracking()
            .Select(CaseServiceDatabaseExpressions.CaseDetail())
            .FirstOrDefaultAsync(cancellation) 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly ITopicProducer<IMarker1> _producer;

    public GetCaseDetailHandler(CaseServiceDbContext dbContext, ITopicProducer<IMarker1> producer)
    {
        _producer = producer;
        _dbContext = dbContext;
    }
}
