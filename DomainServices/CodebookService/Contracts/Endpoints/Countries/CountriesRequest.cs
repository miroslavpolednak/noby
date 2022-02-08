using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Countries
{
    [DataContract]
    public class CountriesRequest : IRequest<List<CountriesItem>>
    {
    }
}
