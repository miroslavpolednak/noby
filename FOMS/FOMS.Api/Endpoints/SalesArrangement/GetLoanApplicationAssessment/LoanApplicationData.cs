using cArrangement = DomainServices.SalesArrangementService.Contracts;
using cHousehold = DomainServices.HouseholdService.Contracts;
using cCase = DomainServices.CaseService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCustomer = DomainServices.CustomerService.Contracts;
using cUser = DomainServices.UserService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment
{

    public class LoanApplicationData
    {

        #region Properties

        /// <summary>
        /// Tatáž verze data (timestamp), která se používá pro LoanApplicationSave a CreateAssesment:
        /// </summary>
        public string LoanApplicationDataVersion { get; init; }

        public cArrangement.SalesArrangement Arrangement { get; init; }

        public cOffer.GetMortgageOfferDetailResponse Offer { get; init; }

        public cCase.Case CaseData { get; init; }

        public cUser.User User { get; init; }

        public List<cHousehold.Household> Households { get; init; }

        public List<cHousehold.CustomerOnSA> CustomersOnSa { get; init; }

        public Dictionary<int, cHousehold.Income> IncomesById { get; init; }

        public Dictionary<string, cCustomer.CustomerDetailResponse> CustomersByIdentityCode { get; init; }

        public Dictionary<int, DomainServices.CodebookService.Contracts.GenericCodebookItem> AcademicDegreesBeforeById { get; init; }

        public Dictionary<int, DomainServices.CodebookService.Contracts.Endpoints.Countries.CountriesItem> CountriesById { get; init; }

        public Dictionary<string, List<int>> ObligationTypeIdsByObligationProperty { get; init; }

        #endregion

        #region Construction

        public LoanApplicationData(
            cArrangement.SalesArrangement arrangement,
            cOffer.GetMortgageOfferDetailResponse offer,
            cUser.User user,
            cCase.Case caseData,
            List<cHousehold.Household> households,
            List<cHousehold.CustomerOnSA> customersOnSa,
            Dictionary<int, cHousehold.Income> incomesById,
            Dictionary<string, cCustomer.CustomerDetailResponse> customersByIdentityCode,
            List<DomainServices.CodebookService.Contracts.GenericCodebookItem> academicDegreesBefore,
            List<DomainServices.CodebookService.Contracts.Endpoints.Countries.CountriesItem> countries,
            List<DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes
            )
        {
            LoanApplicationDataVersion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            Arrangement = arrangement;
            Offer = offer;
            CaseData = caseData;
            User = user;
            Households = households;
            CustomersOnSa = customersOnSa;
            IncomesById = incomesById;
            CustomersByIdentityCode = customersByIdentityCode;
            AcademicDegreesBeforeById = academicDegreesBefore.ToDictionary(i => i.Id);
            CountriesById = countries.ToDictionary(i => i.Id);
            ObligationTypeIdsByObligationProperty = obligationTypes.GroupBy(i => i.ObligationProperty).ToDictionary(i => i.Key, l => l.Select(i => i.Id).ToList());
        }

        #endregion

    }
}
