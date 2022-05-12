using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
  /// <summary>
  /// Amount
  /// </summary>
  public class Amount 
  {
        /// <summary>
        /// Hodnota částky
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Kód měny částky (ISO 4217)
        /// </summary>
        public string CurrencyCode { get; set; }
  }
}
