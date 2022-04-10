
namespace DomainServices.CodebookService.Contracts.Endpoints.JobTypes;

[DataContract]
public sealed class JobTypesRequest : IRequest<List<JobTypeItem>> { }
