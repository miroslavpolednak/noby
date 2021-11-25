﻿using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ActionCodesSavings
{
    [DataContract]
    public class ActionCodesSavingsRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
