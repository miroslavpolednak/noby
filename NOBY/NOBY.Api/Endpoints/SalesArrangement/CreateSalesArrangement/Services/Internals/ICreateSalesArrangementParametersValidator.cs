namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

internal interface ICreateSalesArrangementParametersValidator
{
    Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default);
}
