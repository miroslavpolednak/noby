using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// 
    /// </summary>
    public class LoanApplicationExpanded : LoanApplicationRequest
    {
        /// <summary>
        /// todoSubresource
        /// </summary>
        /// <value>todoSubresource</value>
        public Dictionary<string, Object> TodoSubresource { get; set; }
    }
}
