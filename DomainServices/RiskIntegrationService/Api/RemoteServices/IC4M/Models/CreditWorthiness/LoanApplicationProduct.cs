namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Parametry potřebné pro výpočet Bonity
    /// </summary>
    public class LoanApplicationProduct
    {
        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>
        /// <value>kód produktového shluku (shluk jednoho produktu).</value>
        public string ProductClusterCode { get; set; }

        /// <summary>
        /// Splatnost úvěru - počet splátek v měsíci.
        /// </summary>
        /// <value>Splatnost úvěru - počet splátek v měsíci.</value>
        public long? Maturity { get; set; }

        /// <summary>
        /// Žádaná roční úroková sazba.
        /// </summary>
        /// <value>Žádaná roční úroková sazba.</value>
        public decimal? InterestRate { get; set; }

        /// <summary>
        /// Požadovaná výše úvěru v Kč.
        /// </summary>
        /// <value>Požadovaná výše úvěru v Kč.</value>
        public long? AmountRequired { get; set; }

        /// <summary>
        /// Požadovaná splátka v Kč.
        /// </summary>
        /// <value>Požadovaná splátka v Kč.</value>
        public long? Annuity { get; set; }

        /// <summary>
        /// Doba fixace v měsících.
        /// </summary>
        /// <value>Doba fixace v měsících.</value>
        public long? FixationPeriod { get; set; }
    }
}
