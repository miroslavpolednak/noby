using CIS.Core.Results;
using System;

namespace DomainServices.ProductService.Abstraction.Services
{
    internal class ProductService : IProductServiceAbstraction
    {
        public Task<IServiceCallResult> CreateProductInstance(long caseId, int productInstanceType)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> GetHousingSavingsInstance(long productInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> GetHousingSavingsInstanceBasicDetail(long productInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> GetProductInstanceList(long caseId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> UpdateHousingSavingsInstance(long productInstanceId)
        {
            throw new NotImplementedException();
        }
    }
}
