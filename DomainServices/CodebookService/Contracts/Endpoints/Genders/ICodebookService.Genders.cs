﻿using DomainServices.CodebookService.Contracts.Endpoints.Genders;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenderItem>> Genders(Endpoints.Genders.GendersRequest request, CallContext context = default);
    }
}
