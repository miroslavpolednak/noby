namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationCollateral
    {
        /// <summary>
        /// ID zajištění v dané evidenci (Bagman,StarBuild, atd.).
        /// </summary>
        /// <value>ID zajištění v dané evidenci (Bagman,StarBuild, atd.).</value>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// Kategorie zajištění.
        /// </summary>
        /// <value>Kategorie zajištění.</value>
        public string CategoryCode { get; set; }

        /// <summary>
        /// prohlášená budoucí hodnota zajištění.
        /// </summary>
        /// <value>prohlášená budoucí hodnota zajištění.</value>
        public Amount AppraisedValue { get; set; }

        /// <summary>
        /// prohlášená budoucí hodnota zajištění v původní měně.
        /// </summary>
        /// <value>prohlášená budoucí hodnota zajištění v původní měně.</value>
        public Amount AppraisedValueOriginal { get; set; }
    }
}
