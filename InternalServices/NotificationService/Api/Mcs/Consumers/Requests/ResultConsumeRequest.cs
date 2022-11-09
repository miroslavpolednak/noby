using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Mcs.Consumers.Requests;

public class ResultConsumeRequest : IRequest<ResultConsumeResponse>
{
    public Message<string, NotificationReport> Message { get; set; }
}