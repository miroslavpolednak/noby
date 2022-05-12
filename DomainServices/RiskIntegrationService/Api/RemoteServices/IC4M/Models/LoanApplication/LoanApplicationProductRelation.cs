namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationProductRelation
    {
        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>
        /// <value>kód produktového shluku (shluk jednoho produktu).</value>
        public ResourceIdentifier ProductId { get; set; }

        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>
        /// <value>kód produktového shluku (shluk jednoho produktu).</value>
        public string ProductType { get; set; }

        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>
        /// <value>kód produktového shluku (shluk jednoho produktu).</value>
        public string RelationType { get; set; }

        /// <summary>
        /// konsolidované/zrušené úvěry.
        /// </summary>
        /// <value>konsolidované/zrušené úvěry.</value>
        public LoanApplicationProductRelationValue Value { get; set; }
    }
}
