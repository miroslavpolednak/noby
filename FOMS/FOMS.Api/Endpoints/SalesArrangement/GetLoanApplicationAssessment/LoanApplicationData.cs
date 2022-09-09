using cArrangement = DomainServices.SalesArrangementService.Contracts;
using cCase = DomainServices.CaseService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCustomer = DomainServices.CustomerService.Contracts;
using cUser = DomainServices.UserService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment
{

    public class LoanApplicationData
    {
        #region Properties

        public cArrangement.SalesArrangement Arrangement { get; init; }
       
        public cOffer.GetMortgageOfferResponse Offer { get; init; }

        public cCase.Case CaseData { get; init; }

        public cUser.User User { get; init; }

        public List<cArrangement.Household> Households { get; init; }

        public List<cArrangement.CustomerOnSA> CustomersOnSa { get; init; }

        public Dictionary<int, cArrangement.Income> IncomesById { get; init; }

        public Dictionary<string, cCustomer.CustomerDetailResponse> CustomersByIdentityCode { get; init; }

        #endregion

        #region Construction

        public LoanApplicationData(
            cArrangement.SalesArrangement arrangement,
            cOffer.GetMortgageOfferResponse offer,
            cUser.User user,
            cCase.Case caseData,
            List<cArrangement.Household> households,
            List<cArrangement.CustomerOnSA> customersOnSa,
            Dictionary<int, cArrangement.Income> incomesById,
            Dictionary<string, cCustomer.CustomerDetailResponse> customersByIdentityCode
            )
        {
            Arrangement = arrangement;
            Offer = offer;
            CaseData = caseData;
            User = user;
            Households = households;
            CustomersOnSa = customersOnSa;
            IncomesById = incomesById;
            CustomersByIdentityCode = customersByIdentityCode;
        }

        #endregion
    }
}
