using CIS.Core.Results;

namespace DomainServices.ProductService.Abstraction;

public interface IProductServiceAbstraction
{
    Task<IServiceCallResult> CreateProductInstance(long caseId, int productInstanceType, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> GetHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> GetHousingSavingsInstanceBasicDetail(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> GetProductInstanceList(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> UpdateHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));
}
