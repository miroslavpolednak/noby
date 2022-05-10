using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{ 
   /// <summary>
  /// AssessmentReasonDetail
  /// </summary>
  public class AssessmentReasonDetail
  {
    /// <summary>
    /// Target
    /// </summary>
    /// <value>Target</value>
    public string Target { get; set; }

    /// <summary>
    /// Desc
    /// </summary>
    /// <value>Desc</value>
    public string Desc { get; set; }

    /// <summary>
    /// RiskScaleValue
    /// </summary>
    /// <value>RiskScaleValue</value>
    public string RiskScaleValue { get; set; }

    /// <summary>
    /// Target
    /// </summary>
    /// <value>Target</value>
    public List<Resource> Resource { get; set; }



  }


}

