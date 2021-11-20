using CIS.Core.Results;

namespace DomainServices.ProductService.Abstraction;

public interface IProductServiceAbstraction
{
    Task<IServiceCallResult> CreateProductInstance(long caseId, int productInstanceType);

    Task<IServiceCallResult> GetHousingSavingsInstance(long productInstanceId);

    Task<IServiceCallResult> GetHousingSavingsInstanceBasicDetail(long productInstanceId);

    Task<IServiceCallResult> GetProductInstanceList(long caseId);

    Task<IServiceCallResult> UpdateHousingSavingsInstance(long productInstanceId);
}
