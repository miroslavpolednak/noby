namespace DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes;

[DataContract]
public class LoanPurposesRequest : IRequest<List<LoanPurposesItem>>
{
}
