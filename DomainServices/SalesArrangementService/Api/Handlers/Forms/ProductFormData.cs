using CIS.Foms.Enums;

using DomainServices.CodebookService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.HouseholdService.Contracts;
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
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;
using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacities;


namespace DomainServices.SalesArrangementService.Api.Handlers.Forms
{

    public class ProductFormData
    {
        #region Properties

        private Dictionary<int, HouseholdTypeItem>? householdTypesById;
        private Dictionary<string, CustomerDetailResponse>? customersByIdentityCode;
        private Dictionary<int, Income>? incomesById;
        private Dictionary<string, LegalCapacityItem>? legalCapacitiesByCode;

        public Contracts.SalesArrangement Arrangement { get; init; }

        public SalesArrangementCategories ArrangementCategory { get; init; }

        public ProductTypeItem ProductType { get; init; }

        public GetMortgageOfferDetailResponse Offer { get; init; }

        public Case CaseData { get; init; }

        public User? User { get; init; }

        public List<Household> Households { get; init; }

        public List<CustomerOnSA> CustomersOnSa { get; init; }

        public List<Income> Incomes { get; init; }
        public Dictionary<int, Income> IncomesById
        {
            get
            {
                incomesById = incomesById ?? Incomes.ToDictionary(i => i.IncomeId);
                return incomesById;
            }
        }

        public List<CustomerDetailResponse> Customers { get; init; }
        public Dictionary<string, CustomerDetailResponse> CustomersByIdentityCode { 
            get
            {
                return customersByIdentityCode ?? Customers.ToDictionary(i => i.Identities.First().ToCode());
            }        
        }

        public List<HouseholdTypeItem> HouseholdTypes { get; init; }
        public Dictionary<int, HouseholdTypeItem> HouseholdTypesById { 
            get {
                return householdTypesById ?? HouseholdTypes.ToDictionary(i => i.Id);
            } 
        }

        public Dictionary<int, GenericCodebookItem> AcademicDegreesBeforeById { get; init; }
        public Dictionary<int, GenderItem> GendersById { get; init; }
        public Dictionary<int, SalesArrangementStateItem> SalesArrangementStatesById { get; init; }
        public List<GenericCodebookItemWithCode> EmploymentTypes { get; init; }
        public Dictionary<int, DrawingDurationItem> DrawingDurationById { get; init; }
        public Dictionary<int, DrawingTypeItem> DrawingTypeById { get; init; }
        public Dictionary<int, CountriesItem> CountriesById { get; init; }
        public Dictionary<string, List<int>> ObligationTypeIdsByObligationProperty { get; init; }

        public List<LegalCapacityItem> LegalCapacities { get; init; }
        public Dictionary<string, LegalCapacityItem> LegalCapacitiesByCode
        {
            get
            {
                return legalCapacitiesByCode ?? LegalCapacities.ToDictionary(i => i.Code.ToString());
            }
        }


        #endregion

        #region Construction

        public ProductFormData(
            Contracts.SalesArrangement arrangement,
            ProductTypeItem productType,
            GetMortgageOfferDetailResponse offer,
            Case caseData,
            User? user,
            List<Household> households,
            List<CustomerOnSA> customersOnSa,
            List<CustomerDetailResponse> customers,
            List<Income> incomes,
            List<HouseholdTypeItem> householdTypes,
            List<GenericCodebookItem> academicDegreesBefore,
            List<GenderItem> genders,
            List<SalesArrangementStateItem> salesArrangementStates,
            List<GenericCodebookItemWithCode> employmentTypes,
            List<DrawingDurationItem> drawingDurations,
            List<DrawingTypeItem> drawingTypes,
            List<CountriesItem> countries,
            List<ObligationTypesItem> obligationTypes,
            List<LegalCapacityItem> legalCapacities)
        {
            Arrangement = arrangement;
            ProductType = productType;
            Offer = offer;
            CaseData = caseData;
            User = user;
            Households = households;
            CustomersOnSa = customersOnSa;
            Customers = customers;
            Incomes = incomes;
            HouseholdTypes = householdTypes;
            AcademicDegreesBeforeById = academicDegreesBefore.ToDictionary(i => i.Id);
            GendersById = genders.ToDictionary(i => i.Id);
            SalesArrangementStatesById = salesArrangementStates.ToDictionary(i => i.Id);
            EmploymentTypes = employmentTypes;
            DrawingDurationById = drawingDurations.ToDictionary(i => i.Id);
            DrawingTypeById = drawingTypes.ToDictionary(i => i.Id);
            CountriesById = countries.ToDictionary(i => i.Id);
            ObligationTypeIdsByObligationProperty = obligationTypes.GroupBy(i => i.ObligationProperty).ToDictionary(i => i.Key, l => l.Select(i => i.Id).ToList());
            LegalCapacities = legalCapacities;
        }

        #endregion

    }
}
