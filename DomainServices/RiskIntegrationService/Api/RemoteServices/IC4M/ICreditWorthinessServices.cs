using System.Threading.Tasks;
using Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness;
using Refit;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M
{
    /// <summary>
    /// C4M service
    /// </summary>
    [Headers("Authorization: Basic")]
    public interface ICreditWorthinessServices
    {
        /// <summary>
        /// Výpočet Bonity
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        [Put("/credit-worthiness")]
        Task<ApiResponse<CreditWorthinessCalculation>> CreditWorthinessCalculation([Body] CreditWorthinessCalculationArguments arguments);
    }
}
