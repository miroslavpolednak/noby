using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

internal interface ISalesArrangementValidationStrategy
{
    Task<ValidateSalesArrangementResponse> Validate(SalesArrangement salesArrangement, CancellationToken cancellationToken);
}