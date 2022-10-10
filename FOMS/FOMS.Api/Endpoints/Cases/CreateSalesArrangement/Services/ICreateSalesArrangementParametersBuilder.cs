namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal interface ICreateSalesArrangementParametersBuilder
{
    Task<DomainServices.SalesArrangementService.Contracts.UpdateSalesArrangementParametersRequest> CreateParameters(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));
}
