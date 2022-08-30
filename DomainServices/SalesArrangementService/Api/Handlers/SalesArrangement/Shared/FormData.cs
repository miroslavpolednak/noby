using DomainServices.CodebookService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.UserService.Contracts;

using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.Genders;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;
using DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;

namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement.Shared
{

    public class FormData
    {
        #region Properties

        public Contracts.SalesArrangement Arrangement { get; init; }
        public ProductTypeItem ProductType { get; init; }
        public GetMortgageOfferDetailResponse Offer { get; init; }
        public Case CaseData { get; init; }
        public User? User { get; init; }
        public List<Contracts.Household> Households { get; init; }
        public List<Contracts.CustomerOnSA> CustomersOnSa { get; init; }
        public Dictionary<int, Income> IncomesById { get; init; }
        public Dictionary<string, CustomerResponse> CustomersByIdentityCode { get; init; }
        public Dictionary<int, GenericCodebookItem> AcademicDegreesBeforeById { get; init; }
        public Dictionary<int, GenderItem> GendersById { get; init; }
        public Dictionary<int, SalesArrangementStateItem> SalesArrangementStatesById { get; init; }
        public List<GenericCodebookItemWithCode> EmploymentTypes { get; init; }
        public Dictionary<int, DrawingTypeItem> DrawingTypeById { get; init; }

        #endregion

        #region Construction

        public FormData(
            Contracts.SalesArrangement arrangement,
            ProductTypeItem productType,
            GetMortgageOfferDetailResponse offer,
            Case caseData,
            User? user,
            List<Contracts.Household> households,
            List<Contracts.CustomerOnSA> customersOnSa,
            Dictionary<int, Income> incomesById,
            Dictionary<string, CustomerResponse> customersByIdentityCode,
            Dictionary<int, GenericCodebookItem> academicDegreesBeforeById,
            Dictionary<int, GenderItem> gendersById,
            Dictionary<int, SalesArrangementStateItem> salesArrangementStatesById,
            List<GenericCodebookItemWithCode> employmentTypes,
            Dictionary<int, DrawingTypeItem> drawingTypeById)
        {
            Arrangement = arrangement;
            ProductType = productType;
            Offer = offer;
            CaseData = caseData;
            User = user;
            Households = households;
            CustomersOnSa = customersOnSa;
            IncomesById = incomesById;
            CustomersByIdentityCode = customersByIdentityCode;
            AcademicDegreesBeforeById = academicDegreesBeforeById;
            GendersById = gendersById;
            SalesArrangementStatesById = salesArrangementStatesById;
            EmploymentTypes = employmentTypes;
            DrawingTypeById = drawingTypeById;
        }

        #endregion

    }
}
