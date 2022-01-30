using CIS.Core.Results;
using Microsoft.Extensions.Logging;

namespace DomainServices.ProductService.Abstraction.Services;

internal class ProductService : IProductServiceAbstraction
{
    public async Task<IServiceCallResult> CreateProductInstance(long caseId, int ProductInstanceTypeId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction CreateProductInstance");
        var result = await _userContext.AddUserContext(async () => await _service.CreateProductInstanceAsync(
            new Contracts.CreateProductInstanceRequest
            {
                CaseId = caseId,
                ProductInstanceTypeId = ProductInstanceTypeId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<long>(result.ProductInstanceId);
    }

    public Task<IServiceCallResult> GetHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> GetHousingSavingsInstanceBasicDetail(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> GetProductInstanceList(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> UpdateHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    private readonly ILogger<ProductService> _logger;
    private readonly Contracts.v1.ProductService.ProductServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public ProductService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<ProductService> logger,
        Contracts.v1.ProductService.ProductServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userAccessor = userAccessor;
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
