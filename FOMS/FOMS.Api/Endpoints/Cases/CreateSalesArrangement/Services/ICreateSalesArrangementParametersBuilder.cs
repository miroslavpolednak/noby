namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal interface ICreateSalesArrangementParametersBuilder
{
    Task<DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken));
}
