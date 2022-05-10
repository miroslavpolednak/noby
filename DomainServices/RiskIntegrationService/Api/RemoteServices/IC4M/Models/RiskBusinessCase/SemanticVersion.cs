using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

  /// <summary>
  /// SemanticVersion
  /// </summary>
  
  public class SemanticVersion {
    /// <summary>
    /// Major part of a version
    /// </summary>
    /// <value>Major part of a version</value>
    public int? Major { get; set; }

    /// <summary>
    /// Minor part of a version
    /// </summary>
    /// <value>Minor part of a version</value>
    public int? Minor { get; set; }

    /// <summary>
    /// Bugfix part of a version
    /// </summary>
    /// <value>Bugfix part of a version</value>
    public int? Bugfix { get; set; }

    /// <summary>
    /// A non-semantic part of a version
    /// </summary>
    /// <value>A non-semantic part of a version</value>
    public string NonSemanticPart { get; set; }

  }
}
