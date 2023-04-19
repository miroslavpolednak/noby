namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

internal interface ICreateSalesArrangementParametersValidator
{
    Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default);
}
