using CIS.Foms.Enums;

using DomainServices.CodebookService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.UserService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.Genders;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;
using DomainServices.CodebookService.Contracts.Endpoints.DrawingDurations;
using DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;

namespace DomainServices.SalesArrangementService.Api.Handlers.Shared
{

    public class FormData
    {
        #region Properties

        public Contracts.SalesArrangement Arrangement { get; init; }
        public SalesArrangementCategories ArrangementCategory { get; init; }
        public ProductTypeItem ProductType { get; init; }
        public GetMortgageOfferDetailResponse Offer { get; init; }
        public Case CaseData { get; init; }
        public GetMortgageResponse? ProductMortgage { get; init; }
        public User? User { get; init; }
        public List<_HO.Household> Households { get; init; }
        public List<_HO.CustomerOnSA> CustomersOnSa { get; init; }
        public Dictionary<int, _HO.Income> IncomesById { get; init; }
        public Dictionary<string, CustomerDetailResponse> CustomersByIdentityCode { get; init; }
        public CustomerDetailResponse? DrawingApplicantCustomer { get; init; }
        public Dictionary<int, GenericCodebookItem> AcademicDegreesBeforeById { get; init; }
        public Dictionary<int, GenderItem> GendersById { get; init; }
        public Dictionary<int, SalesArrangementStateItem> SalesArrangementStatesById { get; init; }
        public List<GenericCodebookItemWithCode> EmploymentTypes { get; init; }
        public Dictionary<int, DrawingDurationItem> DrawingDurationById { get; init; }
        public Dictionary<int, DrawingTypeItem> DrawingTypeById { get; init; }
        public Dictionary<int, CountriesItem> CountriesById { get; init; }
        public Dictionary<string, List<int>> ObligationTypeIdsByObligationProperty { get; init; }

        #endregion

        #region Construction

        public FormData(
            Contracts.SalesArrangement arrangement,
            SalesArrangementCategories arrangementCategory,
            ProductTypeItem productType,
            GetMortgageOfferDetailResponse offer,
            Case caseData,
            GetMortgageResponse? productMortgage,
            User? user,
            List<_HO.Household> households,
            List<_HO.CustomerOnSA> customersOnSa,
            Dictionary<int, _HO.Income> incomesById,
            Dictionary<string, CustomerDetailResponse> customersByIdentityCode,
            CustomerDetailResponse? drawingApplicantCustomer,
            List<GenericCodebookItem> academicDegreesBefore,
            List<GenderItem> genders,
            List<SalesArrangementStateItem> salesArrangementStates,
            List<GenericCodebookItemWithCode> employmentTypes,
            List<DrawingDurationItem> drawingDurations,
            List<DrawingTypeItem> drawingTypes,
            List<CountriesItem> countries,
            List<ObligationTypesItem> obligationTypes)
        {
            Arrangement = arrangement;
            ArrangementCategory = arrangementCategory;
            ProductType = productType;
            Offer = offer;
            CaseData = caseData;
            ProductMortgage = productMortgage;
            User = user;
            Households = households;
            CustomersOnSa = customersOnSa;
            IncomesById = incomesById;
            CustomersByIdentityCode = customersByIdentityCode;
            DrawingApplicantCustomer = drawingApplicantCustomer;
            AcademicDegreesBeforeById = academicDegreesBefore.ToDictionary(i => i.Id);
            GendersById = genders.ToDictionary(i => i.Id);
            SalesArrangementStatesById = salesArrangementStates.ToDictionary(i => i.Id);
            EmploymentTypes = employmentTypes;
            DrawingDurationById = drawingDurations.ToDictionary(i => i.Id);
            DrawingTypeById = drawingTypes.ToDictionary(i => i.Id);
            CountriesById = countries.ToDictionary(i => i.Id);
            ObligationTypeIdsByObligationProperty = obligationTypes.GroupBy(i => i.ObligationProperty).ToDictionary(i => i.Key, l => l.Select(i => i.Id).ToList());
        }

        #endregion

    }
}
