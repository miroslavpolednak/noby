using DomainServices.CodebookService.Clients;
using Codebook = DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public class CodebookManager : ICodebookManagerConfigurator
{
    private readonly List<Func<ICodebookServiceClient, CancellationToken, Task>> _codebooksToLoad = new();

    public List<Codebook.CountriesResponse.Types.CountryItem> Countries { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> DegreesBefore { get; private set; } = null!;

    public List<Codebook.LoanPurposesResponse.Types.LoanPurposeItem> LoanPurposes { get; private set; } = null!;

    public List<Codebook.GenericCodebookFullResponse.Types.GenericCodebookFullItem> LoanKinds { get; private set; } = null!;

    public List<Codebook.GenericCodebookFullResponse.Types.GenericCodebookFullItem> RealEstateTypes { get; private set; } = null!;

    public List<Codebook.GenericCodebookFullResponse.Types.GenericCodebookFullItem> PurchaseTypes { get; private set; } = null!;

    public List<Codebook.ProductTypesResponse.Types.ProductTypeItem> ProductTypes { get; private set; } = null!;

    public List<Codebook.PropertySettlementsResponse.Types.PropertySettlementItem> PropertySettlements { get; private set; } = null!;

    public List<Codebook.IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> IdentificationDocumentTypes { get; private set; } = null!;

    public List<Codebook.MaritalStatusesResponse.Types.MaritalStatuseItem> MaritalStatuses { get; private set; } = null!;

    public List<Codebook.DrawingTypesResponse.Types.DrawingTypeItem> DrawingTypes { get; private set; } = null!;

    public List<Codebook.DrawingDurationsResponse.Types.DrawingDurationItem> DrawingDurations { get; private set; } = null!;

    public List<Codebook.SalesArrangementStatesResponse.Types.SalesArrangementStateItem> SalesArrangementStates { get; private set; } = null!;

    public List<Codebook.SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> SalesArrangementTypes { get; private set; } = null!;

    public List<Codebook.GendersResponse.Types.GenderItem> Genders { get; private set; } = null!;

    public List<Codebook.GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem> EmploymentTypes { get; private set; } = null!;

    public List<Codebook.ObligationTypesResponse.Types.ObligationTypeItem> ObligationTypes { get; private set; } = null!;

    public List<Codebook.LegalCapacityRestrictionTypesResponse.Types.LegalCapacityRestrictionTypeItem> LegalCapacityRestrictionTypes { get; private set; } = null!;

    public List<Codebook.DocumentTypesResponse.Types.DocumentTypeItem> DocumentTypes { get; private set; } = null!;

    public List<Codebook.EducationLevelsResponse.Types.EducationLevelItem> EducationLevels { get; private set; } = null!;

    public List<Codebook.GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem> SignatureTypes { get; private set; } = null!;

    public Task Load(ICodebookServiceClient codebookService, CancellationToken cancellationToken)
    {
        return Task.WhenAll(_codebooksToLoad.Select(call => call(codebookService, cancellationToken)));
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.Countries()
    {
        _codebooksToLoad.Add(async (service, ct) => Countries = await service.Countries(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.DegreesBefore()
    {
        _codebooksToLoad.Add(async (service, ct) => DegreesBefore = await service.AcademicDegreesBefore(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.LoanPurposes()
    {
        _codebooksToLoad.Add(async (service, ct) => LoanPurposes = await service.LoanPurposes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.LoanKinds()
    {
        _codebooksToLoad.Add(async (service, ct) => LoanKinds = await service.LoanKinds(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.RealEstateTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => RealEstateTypes = await service.RealEstateTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.PurchaseTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => PurchaseTypes = await service.RealEstatePurchaseTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.ProductTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => ProductTypes = await service.ProductTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.PropertySettlements()
    {
        _codebooksToLoad.Add(async (service, ct) => PropertySettlements = await service.PropertySettlements(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.IdentificationDocumentTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => IdentificationDocumentTypes = await service.IdentificationDocumentTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.MaritalStatuses()
    {
        _codebooksToLoad.Add(async (service, ct) => MaritalStatuses = await service.MaritalStatuses(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.DrawingTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => DrawingTypes = await service.DrawingTypes(ct));

        return this;
    }   
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.DrawingDurations()
    {
        _codebooksToLoad.Add(async (service, ct) => DrawingDurations = await service.DrawingDurations(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.SalesArrangementStates()
    {
        _codebooksToLoad.Add(async (service, ct) => SalesArrangementStates = await service.SalesArrangementStates(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.SalesArrangementTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => SalesArrangementTypes = await service.SalesArrangementTypes(ct));

        return this;
    }    
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.Genders()
    {
        _codebooksToLoad.Add(async (service, ct) => Genders = await service.Genders(ct));

        return this;
    }   
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.EmploymentTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => EmploymentTypes = await service.EmploymentTypes(ct));

        return this;
    }  
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.ObligationTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => ObligationTypes = await service.ObligationTypes(ct));

        return this;
    }  
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.LegalCapacityRestrictionTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => LegalCapacityRestrictionTypes = await service.LegalCapacityRestrictionTypes(ct));

        return this;
    }
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.DocumentTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => DocumentTypes = await service.DocumentTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.EducationLevels()
    {
        _codebooksToLoad.Add(async (service, ct) => EducationLevels = await service.EducationLevels(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.SignatureTypes()
    {
        _codebooksToLoad.Add(async (service, ct) => SignatureTypes = await service.SignatureTypes(ct));

        return this;
    }
}