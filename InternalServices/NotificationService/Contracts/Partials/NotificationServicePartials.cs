﻿using CIS.InternalServices.NotificationService.v2;
using Google.Protobuf.WellKnownTypes;

namespace CIS.InternalServices.NotificationService.Contracts;

public partial class SendSmsRequest
    : MediatR.IRequest<NotificationIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SendSmsFromTemplateRequest
    : MediatR.IRequest<NotificationIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SendEmailRequest
    : MediatR.IRequest<NotificationIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SendEmailFromTemplate
    : MediatR.IRequest<NotificationIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetResultRequest
    : MediatR.IRequest<ResultData>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SearchResultsRequest
    : MediatR.IRequest<SearchResultsResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetStatisticsRequest
    : MediatR.IRequest<GetStatisticsResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetDetailedStatisticsRequest
    : MediatR.IRequest<GetDetailedStatisticsResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class ResendRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }
