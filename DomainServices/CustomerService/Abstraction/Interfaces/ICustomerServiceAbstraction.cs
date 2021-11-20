using CIS.Core.Results;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Abstraction
{
    public interface ICustomerServiceAbstraction
    {
        Task<IServiceCallResult> GetBasicDataByIdentifier(GetBasicDataByIdentifierRequest request);

        Task<IServiceCallResult> GetBasicDataByFullIdentification(GetBasicDataByFullIdentificationRequest request);

        Task<IServiceCallResult> GetDetail(GetDetailRequest request);

        Task<IServiceCallResult> GetList(GetListRequest request);

        Task<IServiceCallResult> Create(CreateRequest request);

        Task<IServiceCallResult> CreateContact(CreateContactRequest request);

        Task<IServiceCallResult> DeleteContact(DeleteContactRequest request);

        Task<IServiceCallResult> UpdateAdress(UpdateAdressRequest request);

        Task<IServiceCallResult> UpdateBasicData(UpdateBasicDataRequest request);
    }
}
