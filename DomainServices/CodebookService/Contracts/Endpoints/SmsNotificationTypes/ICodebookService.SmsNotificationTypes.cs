﻿using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<SmsNotificationTypeItem>> SmsNotificationTypes(SmsNotificationTypesRequest request, CallContext context = default);
    }
}
