namespace DomainServices.CodebookService.Contracts.Endpoints.TinFormatsByCountry;

[DataContract]
public class TinFormatsByCountryRequest : IRequest<List<TinFormatItem>>
{
}
