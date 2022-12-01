using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;

public class ResultConsumeRequest : IRequest<ResultConsumeResponse>
{
    public Message<string, NotificationReport> Message { get; set; } = null!;
}