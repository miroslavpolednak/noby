namespace DomainServices.CodebookService.Contracts.Endpoints.CustomerProfiles;

[DataContract]
public class CustomerProfilesRequest : IRequest<List<CustomerProfileItem>>
{
}