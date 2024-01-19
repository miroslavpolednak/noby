using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Statistics;

[ProtoContract]
public class GetStatisticsRequest
    : IRequest<GetStatisticsResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public List<NotificationState>? States { get; set; }

    [ProtoMember(2)]
    public List<NotificationChannel>? Channels { get; set; }

    [ProtoMember(3)]
    public DateTime? TimeFrom { get; set; }

    [ProtoMember(4)]
    public DateTime? TimeTo { get; set;}
}
