using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationEmploymentIncome
    /// </summary>
    public class LoanApplicationEmploymentIncome
    {
        /// <summary>
        /// IDDI zaměstnavatele.
        /// </summary>
        /// <value>IDDI zaměstnavatele.</value>
        public ResourceIdentifier EmployerId { get; set; }

        /// <summary>
        /// rodné číslo / IČO zaměstnavatele.
        /// </summary>
        /// <value>rodné číslo / IČO zaměstnavatele.</value>
        public string EmployerIdentificationNumber { get; set; }

        /// <summary>
        /// Typ zaměstnavatele - státní společnost, spol. se zahraniční účastí, podnikatel, etc..
        /// </summary>
        /// <value>Typ zaměstnavatele - státní společnost, spol. se zahraniční účastí, podnikatel, etc..</value>
        public long? EmployerType { get; set; }

        /// <summary>
        /// Název zaměstnavatele
        /// </summary>
        /// <value>Název zaměstnavatele</value>
        public string EmployerName { get; set; }

        /// <summary>
        /// agregovaný OKEČ zaměstnavatele.
        /// </summary>
        /// <value>agregovaný OKEČ zaměstnavatele.</value>
        public long? Nace { get; set; }

        /// <summary>
        /// povolání.
        /// </summary>
        /// <value>povolání.</value>
        public long? Profession { get; set; }

        /// <summary>
        /// PSČ sídla zaměstnavatele.
        /// </summary>
        /// <value>PSČ sídla zaměstnavatele.</value>
        public long? Postcode { get; set; }

        /// <summary>
        /// město sídla zaměstnání.
        /// </summary>
        /// <value>město sídla zaměstnání.</value>
        public string City { get; set; }

        /// <summary>
        /// Stát sídla zaměstnání.
        /// </summary>
        /// <value>Stát sídla zaměstnání.</value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Datum začátek zaměstnaní.
        /// </summary>
        /// <value>Datum začátek zaměstnaní.</value>
        public DateTime? EmployedFrom { get; set; }

        /// <summary>
        /// Název pracovní pozice.
        /// </summary>
        /// <value>Název pracovní pozice.</value>
        public string JobTitle { get; set; }

        /// <summary>
        /// ulice sídla zaměstnání.
        /// </summary>
        /// <value>ulice sídla zaměstnání.</value>
        public string Street { get; set; }

        /// <summary>
        /// číslo katastru nemovitostí.
        /// </summary>
        /// <value>číslo katastru nemovitostí.</value>
        public string LandRegisterNumber { get; set; }

        /// <summary>
        /// Telefonní kontakt.
        /// </summary>
        /// <value>Telefonní kontakt.</value>
        public PhoneContact Phone { get; set; }

      
        /// <summary>
        /// Název pracovní pozice.
        /// </summary>
        /// <value>Název pracovní pozice.</value>
        /// public string JobTitleEmployee { get; set; }

        /// <summary>
        /// čistý průměrný měsíční příjem ze závislé činnosti za posledních 12 měsíců (nejméně 3 měsíce).
        /// </summary>
        /// <value>čistý průměrný měsíční příjem ze závislé činnosti za posledních 12 měsíců (nejméně 3 měsíce).</value>
        public Amount MonthlyIncomeAmount { get; set; }

        /// <summary>
        /// Číslo účtu, ze kterého od daného zaměstnavatele chodí mzda.
        /// </summary>
        /// <value>Číslo účtu, ze kterého od daného zaměstnavatele chodí mzda.</value>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Je příjem domicilován?.
        /// </summary>
        /// <value>Je příjem domicilován?.</value>
        public bool? Domiciled { get; set; }


        /// <summary>
        /// typ dokumentu dokládajícího příjmy (\"PNZ\"=příjem není zadán).
        /// </summary>
        /// <value>typ dokumentu dokládajícího příjmy (\"PNZ\"=příjem není zadán).</value>
        public string ProofType { get; set; }

        /// <summary>
        /// domicilovaný příjem deklarovaný klientem.
        /// </summary>
        /// <value>domicilovaný příjem deklarovaný klientem.</value>
        public decimal? DeclaredMonthIncome { get; set; }

        /// <summary>
        /// Typ zaměstnání.
        /// </summary>
        /// <value>Typ zaměstnání.</value>
        public string ForeignEmploymentType { get; set; }


        /// <summary>
        /// Hrubý roční příjem bez pojistného za poslední zdaňovací období.
        /// </summary>
        /// <value>Hrubý roční příjem bez pojistného za poslední zdaňovací období.</value>
        public decimal? GrossAnnualIncome { get; set; }

        /// <summary>
        /// kontaktní osoba uvedená na potvrzení o výši příjmu.
        /// </summary>
        /// <value>kontaktní osoba uvedená na potvrzení o výši příjmu.</value>
        public string ProofConfirmationContactSurname { get; set; }

        /// <summary>
        /// kontaktní tel. číslo uvedené na potvrzení o výši příjmu.
        /// </summary>
        /// <value>kontaktní tel. číslo uvedené na potvrzení o výši příjmu.</value>
        public string ProofConfirmationContactPhone { get; set; }

        /// <summary>
        /// Datum vystavení potvrzení o výši příjmu.
        /// </summary>
        /// <value>Datum vystavení potvrzení o výši příjmu.</value>
        public DateTime? ProofCreatedOn { get; set; }

        /// <summary>
        /// Je klient ve zkušební době?.
        /// </summary>
        /// <value>Je klient ve zkušební době?.</value>
        public bool? ProbationaryPeriod { get; set; }

        /// <summary>
        /// Je klient ve výpovědní lhůtě?.
        /// </summary>
        /// <value>Je klient ve výpovědní lhůtě?.</value>
        public bool? NoticePeriod { get; set; }

        /// <summary>
        /// místo vystavení potvrzení o příjmu.
        /// </summary>
        /// <value>místo vystavení potvrzení o příjmu.</value>
        /// public string ProofIssuePlace { get; set; }
                      

        /// <summary>
        /// Je pracovní poměr sjednán na dobu určitou?.
        /// </summary>
        /// <value>Je pracovní poměr sjednán na dobu určitou?.</value>
        public bool? FixedTerm { get; set; }

        /// <summary>
        /// první pracovní smlouva se zaměstnavatelem od.
        /// </summary>
        /// <value>první pracovní smlouva se zaměstnavatelem od.</value>
        public DateTime? FirstContractFrom { get; set; }

        /// <summary>
        /// aktuální pracovní smlouva od.
        /// </summary>
        /// <value>aktuální pracovní smlouva od.</value>
        public DateTime? CurrentContractFrom { get; set; }

        /// <summary>
        /// aktuální pracovní smlouva do.
        /// </summary>
        /// <value>aktuální pracovní smlouva do.</value>
        public DateTime? CurrentContractTo { get; set; }

        /// <summary>
        /// Je požadavek na prioritní verifikaci příjmu?
        /// </summary>
        /// <value>Je požadavek na prioritní verifikaci příjmu?</value>
        public bool? VerificationPriority { get; set; }

        /// <summary>
        /// Vystavila potvrzení o příjmu externí účetní firma?
        /// </summary>
        /// <value>Vystavila potvrzení o příjmu externí účetní firma?</value>
        public bool? IssuedByExternalAccountant { get; set; }

        /// <summary>
        /// Z příjmu Zaměstnance  jsou/nejsou prováděny srážky.
        /// </summary>
        /// <value>Z příjmu Zaměstnance  jsou/nejsou prováděny srážky.</value>
        public List<IncomeDeduction> IncomeDeduction { get; set; }
    }
}
