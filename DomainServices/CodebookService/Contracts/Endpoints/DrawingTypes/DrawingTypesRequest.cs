using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes
{
    [DataContract]
    public class DrawingTypesRequest : IRequest<List<DrawingTypeItem>>
    {
    }
}
