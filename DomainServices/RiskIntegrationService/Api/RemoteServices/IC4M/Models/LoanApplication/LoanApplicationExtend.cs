using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    internal class LoanApplicationExtend
    {
        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// rozlišení nového obchodu a typu dodatku
        /// </summary>
        /// <value>rozlišení nového obchodu a typu dodatku</value>
        public long? AppendixCode { get; set; }

        /// <summary>
        /// distribuční kanál vzniku žádosti
        /// </summary>
        /// <value>distribuční kanál vzniku žádosti</value>
        public string DistributionChannelCode { get; set; }

        /// <summary>
        /// Způsob podpisu žádosti
        /// </summary>
        /// <value>Způsob podpisu žádosti</value>
        public string SignatureType { get; set; }

        /// <summary>
        /// loan application verze
        /// </summary>
        /// <value>loan application verze</value>
        public string LoanApplicationDataVersion { get; set; }

        /// <summary>
        /// pouze CREATOR
        /// </summary>
        /// <value>pouze CREATOR</value>
        public KBGroupPerson Person { get; set; }

        /// <summary>
        /// aplikačních data jednotlivých domácností (včetně ručitelských)
        /// </summary>
        /// TODO : doplnit datovy typ LoanApplicationHouseholdExtend
        /// <value>aplikačních data jednotlivých domácností (včetně ručitelských)</value>
        //  public List<LoanApplicationHouseholdExtend> LoanApplicationHousehold { get; set; }

        /// <summary>
        /// parametrů jednotlivých účtů balíčku
        /// </summary>
        /// <value>parametrů jednotlivých účtů balíčku</value>
        public LoanApplicationProduct LoanApplicationProduct { get; set; }

        /// <summary>
        /// konsolidované/zrušené úvěry 
        /// </summary>
        /// <value>konsolidované/zrušené úvěry </value>
        public List<LoanApplicationProductRelation> LoanApplicationProductRelation { get; set; }

        /// <summary>
        /// Gets or Sets LoanApplicationDealer
        /// </summary>
        public LoanApplicationDealer LoanApplicationDealer { get; set; }


    }
}
