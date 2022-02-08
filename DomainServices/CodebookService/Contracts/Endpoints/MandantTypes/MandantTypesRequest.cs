using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.MandantTypes
{
    [DataContract]
    public class MandantTypesRequest : IRequest<List<MandantTypesItem>>
    {
    }
}
