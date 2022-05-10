using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// RiskBusinessCaseCommandState
    /// </summary>

  public class RiskBusinessCaseCommandState
  {
    /// <summary>
    /// ID založeného požadavku 
    /// </summary>
    /// <value>ID založeného požadavku </value>
    public long? CommandId { get; set; }

    /// <summary>
    /// stateCode 
    /// </summary>
    /// <value>stateCode </value>
    public string StateCode { get; set; }

    /// <summary>
    /// loanApplicationAssessmentId
    /// </summary>
    /// <value>loanApplicationAssessmentId</value>
    public long? LoanApplicationAssessmentId { get; set; }
 
  }
}
