namespace DomainServices.CodebookService.Contracts.Endpoints.EmploymentTypes;

[DataContract]
public class EmploymentTypesRequest : IRequest<List<GenericCodebookItemWithCode>>
{
}
