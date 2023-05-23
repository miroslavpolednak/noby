using cArrangement = DomainServices.SalesArrangementService.Contracts;
using cHousehold = DomainServices.HouseholdService.Contracts;
using cCase = DomainServices.CaseService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCustomer = DomainServices.CustomerService.Contracts;
using cUser = DomainServices.UserService.Contracts;
using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment
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

        public Dictionary<int, GenericCodebookResponse.Types.GenericCodebookItem> AcademicDegreesBeforeById { get; init; }

        public Dictionary<int, CountriesResponse.Types.CountryItem> CountriesById { get; init; }

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
            List<GenericCodebookResponse.Types.GenericCodebookItem> academicDegreesBefore,
            List<CountriesResponse.Types.CountryItem> countries,
            List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes
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
