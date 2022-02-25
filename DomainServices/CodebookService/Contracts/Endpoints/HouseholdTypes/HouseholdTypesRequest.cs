namespace DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

[DataContract]
public class HouseholdTypesRequest : IRequest<List<HouseholdTypeItem>>
{
}