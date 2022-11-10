namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal interface ICreateSalesArrangementParametersValidator
{
    Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken));
}
