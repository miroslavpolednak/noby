using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

internal interface ICreateSalesArrangementParametersBuilder
{
    Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default);

    Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default);
}
