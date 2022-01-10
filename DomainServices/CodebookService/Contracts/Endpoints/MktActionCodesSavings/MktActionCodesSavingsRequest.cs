using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.MktActionCodesSavings
{
    [DataContract]
    public partial class MktActionCodesSavingsRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
