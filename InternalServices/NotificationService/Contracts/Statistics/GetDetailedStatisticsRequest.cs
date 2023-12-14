﻿using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics;

[ProtoContract]
public class GetDetailedStatisticsRequest
    : IRequest<GetDetailedStatisticsResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public List<NotificationState>? States { get; set; }

    [ProtoMember(2)]
    public List<NotificationChannel>? Channels { get; set; }

    [ProtoMember(3)]
    public DateTime StatisticsDate { get; set; }
}