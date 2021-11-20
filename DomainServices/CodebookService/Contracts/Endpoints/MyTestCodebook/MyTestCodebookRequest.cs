using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.MyTestCodebook
{
    /// <summary>
    /// Dto objekt musi implementovat MediatR interface IRequest[] s korektni signaturou navratoveho typu, jinak nedojde ke spusteni handleru
    /// </summary>
    [DataContract]
    public sealed class MyTestCodebookRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
