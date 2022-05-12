using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationCounterParty
    {
        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public long? Id { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public bool? IsPartner { get; set; }


        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public ResourceIdentifier CustomerId { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public bool? GroupEmployee { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public bool? SpecialRelationsWithKB { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string BirthNumber { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string RoleCode { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string Firstname { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string Surname { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string BirthName { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string BirthPlace { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public PrimaryAddress Address { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string Gender { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public long? HighestEducation { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string AcademicTitlePrefix { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public List<PhoneContact> Phone { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public bool? HasEmail { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string HousingCondition { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string SegmentCode { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public LoanApplicationIncome Income { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public bool? Taxpayer { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public string CounterpartyType { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public LoanApplicationPersonalDocument LoanApplicationPersonalDocument { get; set; }

        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public LoanApplicationCounterpartyConsent LoanApplicationCounterpartyConsent { get; set; }

        /// <summary>
        /// Gets or Sets ManagementType
        /// </summary>
        public string ManagementType { get; set; }

        /// <summary>
        /// LoanApplicationDeclaredIncome
        /// </summary>
        /// TODO : doplnit typ  LoanApplicationDeclaredIncome
        public LoanApplicationDeclaredIncome LoanApplicationDeclaredIncome { get; set; }



    }
}
