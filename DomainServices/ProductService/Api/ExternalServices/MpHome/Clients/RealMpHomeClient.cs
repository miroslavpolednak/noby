namespace DomainServices.ProductService.Api.MpHome;

public class RealMpHomeClient : IMpHomeClient
{
    public async Task<CIS.Core.Results.IServiceCallResult> CreateSavingsInstance(long productInstanceId)
    {
        return new CIS.Core.Results.SuccessfulServiceCallResult();
    }

    public async Task<CIS.Core.Results.IServiceCallResult> CreateSavingsLoanInstance(long productInstanceId)
    {
        return new CIS.Core.Results.SuccessfulServiceCallResult();
    }

    public async Task<CIS.Core.Results.IServiceCallResult> CreateMorgageInstance(long productInstanceId)
    {
        return new CIS.Core.Results.SuccessfulServiceCallResult();
    }

    public async Task<CIS.Core.Results.IServiceCallResult> GetProductInstanceList(long caseId)
    {
        return new CIS.Core.Results.SuccessfulServiceCallResult();
    }

    public async Task<CIS.Core.Results.IServiceCallResult> UpdateSavingsInstance()
    {
        return new CIS.Core.Results.SuccessfulServiceCallResult();
    }
}
