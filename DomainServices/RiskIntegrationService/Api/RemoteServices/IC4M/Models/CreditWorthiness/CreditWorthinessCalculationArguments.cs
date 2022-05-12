namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Vstupní parametry potřebné pro výpočet Bonity
    /// </summary>
    public class CreditWorthinessCalculationArguments
    {
        /// <summary>
        /// Id
        /// </summary>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// ResourceProcessId
        /// </summary>
        public ResourceIdentifier ResourceProcessId { get; set; }

        /// <summary>
        /// Identifikátor volající aplikace.
        /// </summary>
        /// <value>Identifikátor volající aplikace.</value>
        public string ItChannel { get; set; }

        /// <summary>
        /// Identifikátor žádosti z pohledu Risku, nepovinné.
        /// </summary>
        /// <value>Identifikátor žádosti z pohledu Risku, nepovinné.</value>
        public long? RiskBusinessCaseId { get; set; }

        /// <summary>
        /// Informace o delaerovi, který službu volá
        /// </summary>
        /// <value>Informace o delaerovi, který službu volá</value>
        public Dealer LoanApplicationDealer { get; set; }

        /// <summary>
        /// Informace o interním zaměstanci, který službu volá
        /// </summary>
        /// <value>Informace o interním zaměstanci, který službu volá</value>
        public Person KbGroupPerson { get; set; }

        /// <summary>
        /// Identifikátor žádosti z pohledu Risku, nepovinné.
        /// </summary>
        /// <value>Identifikátor žádosti z pohledu Risku, nepovinné.</value>
        public LoanApplicationProduct LoanApplicationProduct { get; set; }

        /// <summary>
        /// Domácnosti.
        /// </summary>
        /// <value>Domácnosti.</value>
        public List<LoanApplicationHousehold> Households { get; set; }
    }
}