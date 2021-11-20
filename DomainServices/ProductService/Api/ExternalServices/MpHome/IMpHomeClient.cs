namespace DomainServices.ProductService.Api.MpHome;

internal interface IMpHomeClient
{
    Task<CIS.Core.Results.IServiceCallResult> CreateSavingsInstance(long productInstanceId);

    Task<CIS.Core.Results.IServiceCallResult> CreateSavingsLoanInstance(long productInstanceId);

    Task<CIS.Core.Results.IServiceCallResult> CreateMorgageInstance(long productInstanceId);

    Task<CIS.Core.Results.IServiceCallResult> GetProductInstanceList(long caseId);

    Task<CIS.Core.Results.IServiceCallResult> UpdateSavingsInstance();
}
