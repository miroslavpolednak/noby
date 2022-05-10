using System.Threading.Tasks;
using Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication;
using Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment;
using Refit;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M
{
    /// <summary>
    /// C4M service
    /// </summary>
    [Headers("Authorization: Basic")]
    public interface ILoanApplicationServices
    {
        /// <summary>
        /// Loan application create
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        [Post("/hf/loan-application")]
        Task<ApiResponse<LoanApplicationResult>> LoanApplicationCreate([Body] LoanApplicationRequest request);


        /// <summary>
        /// Loan application create
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        [Post("/la-loan-application-hf-adapter-service-0.1.0")]
        Task<ApiResponse<LoanApplicationResult>> LoanApplicationAssessment([Body] LoanApplicationRequest request);
    }
}