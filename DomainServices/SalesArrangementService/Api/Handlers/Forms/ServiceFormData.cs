using DomainServices.CodebookService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.Forms
{
    public class ServiceFormData
    {
        #region Properties

        private Dictionary<int, GenericCodebookItem>? academicDegreesBeforeById;

        public Contracts.SalesArrangement Arrangement { get; init; }

        public Case CaseData { get; init; }

        public GetMortgageResponse? ProductMortgage { get; init; }

        public CustomerDetailResponse? DrawingApplicantCustomer { get; init; }

        public List<GenericCodebookItem> AcademicDegreesBefore { get; init; }
        public Dictionary<int, GenericCodebookItem> AcademicDegreesBeforeById { 
            get {
                academicDegreesBeforeById = academicDegreesBeforeById ?? AcademicDegreesBefore.ToDictionary(i => i.Id);
                return academicDegreesBeforeById;
            } 
        }

        #endregion

        #region Construction

        public ServiceFormData(
            Contracts.SalesArrangement arrangement,
            Case caseData,
            GetMortgageResponse? productMortgage,
            CustomerDetailResponse? drawingApplicantCustomer,
            List<GenericCodebookItem> academicDegreesBefore
            )
        {
            Arrangement = arrangement;
            CaseData = caseData;
            ProductMortgage = productMortgage;
            DrawingApplicantCustomer = drawingApplicantCustomer;
            AcademicDegreesBefore = academicDegreesBefore;
        }

        #endregion

    }
}
