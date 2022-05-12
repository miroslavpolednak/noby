using C4M = Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RipService.CreditWorthiness;

/// <summary>
/// Response transformation (C4M->RIP)
/// </summary>
[CIS.Infrastructure.Attributes.ScopedService]
public class CreditWorthinessComputeResponseTransformation : ICreditWorthinessComputeResponseTransformation
{
    private readonly ILogger<CreditWorthinessComputeResponseTransformation> _logger;
    private Contracts.CreditWorthinessResponse RipResponse { get; set; } = new Contracts.CreditWorthinessResponse();
    private Contracts.CreditWorthinessRequest RipRequest { get; set; }


    public CreditWorthinessComputeResponseTransformation(ILogger<CreditWorthinessComputeResponseTransformation> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Transform response (C4M->RIP)
    /// </summary>
    /// <param name="c4mResponse"></param>
    /// <returns>RIP.LoanApplicationCreate</returns>
    public Contracts.CreditWorthinessResponse Transform(C4M.CreditWorthinessCalculation c4mResult, Contracts.CreditWorthinessRequest ripRequest)
    {
        RipRequest = ripRequest;
                        
        RipResponse.RemainsLivingInst = c4mResult.RemainsLivingInst;
        RipResponse.InstallmentLimit = c4mResult.InstallmentLimit;
        RipResponse.RemainsLivingAnnuity = c4mResult.RemainsLivingAnnuity;
        RipResponse.MaxAmount = c4mResult.MaxAmount;
        //RipResponse.ResultReason.Code = c4mResult.ResultReason?.Code;
        //RipResponse.ResultReason.Description = c4mResult.ResultReason?.Description;
        RipResponse.WorthinessResult = getWorthinessResult(c4mResult);

        return RipResponse;
    }
    private int getWorthinessResult(C4M.CreditWorthinessCalculation c4mResult)
    {
        int WorthinessResult = 0;
        if (c4mResult.InstallmentLimit > RipRequest.LoanApplicationProduct.Annuity)
        {
            WorthinessResult = 1;
        }
        return (WorthinessResult);
    }
}
