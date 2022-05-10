using System.Collections.Generic;

using System.Threading.Tasks;
using Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase;
using Refit;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M
{
    /// <summary>
    /// C4M services
    /// </summary>
    [Headers("Authorization: Basic")]
    public interface IRiskBusinessCaseServices
    {
        /// <summary>
        /// RiskBusinesCase Assessment
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns>RiskBusinessCaseCommand</returns>
        [Post("/assessment/command/riskBusinessCaseId={id}")]
        Task<ApiResponse<RiskBusinessCaseCommand>> RiskBusinessCaseAssessment(string id, [Body] AssessmentRequest request);

        /// <summary>
        /// RiskBusinesCase Create
        /// </summary>
        /// <param name="request"></param>   
        /// <returns>LoanApplicationCreate</returns>
        [Post("/riskBusinessCase")]
        Task<ApiResponse<LoanApplicationCreate>> RiskBusinessCase([Body] CreateRequest request);

        /// <summary>
        /// RiskBusinesCase Commit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>RiskBusinessCaseCommitResponse</returns>
        [Put("/riskBusinessCase/{id}/commitment")]
        Task<ApiResponse<LoanApplicationCommit>> RiskBusinessCaseCommit(string id, [Body] CommitRequest request);
        //TODO: aktualizovat URI
    }
}