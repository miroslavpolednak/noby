using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using cz.kb.osbs.mcs.sender.sendapi.v4;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Messaging;
using DomainServices.CaseService.Contracts;
using MassTransit;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

internal class GetCaseDetailHandler
    : IRequestHandler<GetCaseDetailRequest, Case>
{
    /// <summary>
    /// Vraci detail Case-u
    /// </summary>
    public async Task<Case> Handle(GetCaseDetailRequest request, CancellationToken cancellation)
    {
        var party = new Party
        {
            legalPerson = new LegalPerson
            {
                name = "NAME"
            }
        };
        var message = new SendEmail
        {
            id = Guid.NewGuid().ToString(),
            attachments = new List<Attachment>(),
            bcc = new List<EmailAddress>(),
            cc = new List<EmailAddress>(),
            content = new Content
            {
                text = "test"
            },
            sender = new EmailAddress { party = party, value = "aaaaa@kb.cz" },
            subject = "test",
            to = new List<EmailAddress>
            {
                new() { party = party, value = "ottik.ottik@gmail.com" }
            },
            notificationConsumer = new () { consumerId = "CONSUMER_ID" },
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

    public GetCaseDetailHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
