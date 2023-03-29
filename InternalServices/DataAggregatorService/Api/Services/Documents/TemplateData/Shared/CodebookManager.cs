using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;
using Codebook = DomainServices.CodebookService.Contracts.Endpoints;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public class CodebookManager : ICodebookManagerConfigurator
{
    private readonly List<Func<ICodebookServiceClients, CancellationToken, Task>> _codebooksToLoad = new();

    public List<Codebook.Countries.CountriesItem> Countries { get; private set; } = null!;

    public List<GenericCodebookItem> DegreesBefore { get; private set; } = null!;

    public List<Codebook.LoanPurposes.LoanPurposesItem> LoanPurposes { get; private set; } = null!;

    public List<Codebook.LoanKinds.LoanKindsItem> LoanKinds { get; private set; } = null!;

    public List<Codebook.RealEstateTypes.RealEstateTypeItem> RealEstateTypes { get; private set; } = null!;

    public List<Codebook.RealEstatePurchaseTypes.RealEstatePurchaseTypeItem> PurchaseTypes { get; private set; } = null!;

    public List<Codebook.ProductTypes.ProductTypeItem> ProductTypes { get; private set; } = null!;

    public List<Codebook.PropertySettlements.PropertySettlementItem> PropertySettlements { get; private set; } = null!;

    public List<Codebook.IdentificationDocumentTypes.IdentificationDocumentTypesItem> IdentificationDocumentTypes { get; private set; } = null!;

    public List<Codebook.MaritalStatuses.MaritalStatusItem> MaritalStatuses { get; private set; } = null!;

    public List<Codebook.DrawingTypes.DrawingTypeItem> DrawingTypes { get; private set; } = null!;

    public List<Codebook.DrawingDurations.DrawingDurationItem> DrawingDurations { get; private set; } = null!;

    public List<Codebook.SalesArrangementStates.SalesArrangementStateItem> SalesArrangementStates { get; private set; } = null!;

    public List<Codebook.SalesArrangementTypes.SalesArrangementTypeItem> SalesArrangementTypes { get; private set; } = null!;

    public List<Codebook.Genders.GenderItem> Genders { get; private set; } = null!;

    public List<GenericCodebookItemWithCode> EmploymentTypes { get; private set; } = null!;

    public List<Codebook.ObligationTypes.ObligationTypesItem> ObligationTypes { get; private set; } = null!;

    public List<Codebook.LegalCapacityRestrictionTypes.LegalCapacityRestrictionTypeItem> LegalCapacityRestrictionTypes { get; private set; } = null!;

    public List<Codebook.DocumentTypes.DocumentTypeItem> DocumentTypes { get; private set; } = null!;

    public Task Load(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
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
}