namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public interface ICodebookManagerConfigurator
{
    ICodebookManagerConfigurator Countries();
    ICodebookManagerConfigurator DegreesBefore();
    ICodebookManagerConfigurator LoanPurposes();
    ICodebookManagerConfigurator LoanKinds();
    ICodebookManagerConfigurator RealEstateTypes();
    ICodebookManagerConfigurator PurchaseTypes();
    ICodebookManagerConfigurator ProductTypes();
    ICodebookManagerConfigurator PropertySettlements();
    ICodebookManagerConfigurator IdentificationDocumentTypes();
    ICodebookManagerConfigurator MaritalStatuses();
    ICodebookManagerConfigurator DrawingTypes();
    ICodebookManagerConfigurator DrawingDurations();
    ICodebookManagerConfigurator SalesArrangementStates();
    ICodebookManagerConfigurator SalesArrangementTypes();
    ICodebookManagerConfigurator Genders();
    ICodebookManagerConfigurator EmploymentTypes();
    ICodebookManagerConfigurator ObligationTypes();
    ICodebookManagerConfigurator LegalCapacityRestrictionTypes();
    ICodebookManagerConfigurator DocumentTypes();
    ICodebookManagerConfigurator EducationLevels();
    ICodebookManagerConfigurator SignatureTypes();
    ICodebookManagerConfigurator TinNoFillReasonsByCountry();
}