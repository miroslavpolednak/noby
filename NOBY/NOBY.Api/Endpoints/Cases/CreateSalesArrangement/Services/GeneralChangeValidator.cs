namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class GeneralChangeValidator
    : BaseValidator, ICreateSalesArrangementParametersValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GeneralChangeValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        return new GeneralChangeBuilder(_logger, _request, _httpContextAccessor);
    }
}
