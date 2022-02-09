using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes
{
    [DataContract]
    public class IdentificationDocumentTypesRequest : IRequest<List<IdentificationDocumentTypesItem>>
    {
    }
}
