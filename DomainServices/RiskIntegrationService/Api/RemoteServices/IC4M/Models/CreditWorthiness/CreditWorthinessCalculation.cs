namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{

    /// <summary>
    /// Vypočtená Bonita
    /// </summary>
    public class CreditWorthinessCalculation
    {
        /// <summary>
        /// maximální disponibilní splátka
        /// </summary>
        public long? InstallmentLimit { get; set; }

        /// <summary>
        /// maximální výše úvěru
        /// </summary>
        public long? MaxAmount { get; set; }

        /// <summary>
        /// Zbývá na živobytí s ANNUITY
        /// </summary>
        public long? RemainsLivingAnnuity { get; set; }

        /// <summary>
        /// Zbývá na živobytí s disp. Splátkou
        /// </summary>
        public long? RemainsLivingInst { get; set; }

        /// <summary>
        /// Gets or Sets ResultReason
        /// </summary>
        public ResultReason ResultReason { get; set; }
    }
}
