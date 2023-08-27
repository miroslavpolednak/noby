using System.Runtime.CompilerServices;
using DomainServices.CodebookService.Clients;
using Codebook = DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public class CodebookManager : ICodebookManagerConfigurator
{
    private readonly Dictionary<string, Func<ICodebookServiceClient, CancellationToken, Task>> _codebooksToLoad = new();

    public List<Codebook.CountriesResponse.Types.CountryItem> Countries { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> DegreesBefore { get; private set; } = null!;

    public List<Codebook.LoanPurposesResponse.Types.LoanPurposeItem> LoanPurposes { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> LoanKinds { get; private set; } = null!;

    public List<Codebook.RealEstateTypesResponse.Types.RealEstateTypesResponseItem> RealEstateTypes { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> PurchaseTypes { get; private set; } = null!;

    public List<Codebook.ProductTypesResponse.Types.ProductTypeItem> ProductTypes { get; private set; } = null!;

    public List<Codebook.PropertySettlementsResponse.Types.PropertySettlementItem> PropertySettlements { get; private set; } = null!;

    public List<Codebook.IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> IdentificationDocumentTypes { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> MaritalStatuses { get; private set; } = null!;

    public List<Codebook.DrawingTypesResponse.Types.DrawingTypeItem> DrawingTypes { get; private set; } = null!;

    public List<Codebook.DrawingDurationsResponse.Types.DrawingDurationItem> DrawingDurations { get; private set; } = null!;

    public List<Codebook.SalesArrangementStatesResponse.Types.SalesArrangementStateItem> SalesArrangementStates { get; private set; } = null!;

    public List<Codebook.SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> SalesArrangementTypes { get; private set; } = null!;

    public List<Codebook.GendersResponse.Types.GenderItem> Genders { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> EmploymentTypes { get; private set; } = null!;

    public List<Codebook.ObligationTypesResponse.Types.ObligationTypeItem> ObligationTypes { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> LegalCapacityRestrictionTypes { get; private set; } = null!;

    public List<Codebook.DocumentTypesResponse.Types.DocumentTypeItem> DocumentTypes { get; private set; } = null!;

    public List<Codebook.EducationLevelsResponse.Types.EducationLevelItem> EducationLevels { get; private set; } = null!;

    public List<Codebook.GenericCodebookResponse.Types.GenericCodebookItem> SignatureTypes { get; private set; } = null!;

    public List<Codebook.TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem> TinNoFillReasonsByCountry { get; private set; } = null!;

    Task ICodebookManagerConfigurator.Load(ICodebookServiceClient codebookService, CancellationToken cancellationToken)
    {
        return Task.WhenAll(_codebooksToLoad.Values.Select(call => call(codebookService, cancellationToken)));
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.Countries()
    {
        AddToLoad(async (service, ct) => Countries = await service.Countries(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.DegreesBefore()
    {
        AddToLoad(async (service, ct) => DegreesBefore = await service.AcademicDegreesBefore(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.LoanPurposes()
    {
        AddToLoad(async (service, ct) => LoanPurposes = await service.LoanPurposes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.LoanKinds()
    {
        AddToLoad(async (service, ct) => LoanKinds = await service.LoanKinds(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.RealEstateTypes()
    {
        AddToLoad(async (service, ct) => RealEstateTypes = await service.RealEstateTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.PurchaseTypes()
    {
        AddToLoad(async (service, ct) => PurchaseTypes = await service.RealEstatePurchaseTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.ProductTypes()
    {
        AddToLoad(async (service, ct) => ProductTypes = await service.ProductTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.PropertySettlements()
    {
        AddToLoad(async (service, ct) => PropertySettlements = await service.PropertySettlements(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.IdentificationDocumentTypes()
    {
        AddToLoad(async (service, ct) => IdentificationDocumentTypes = await service.IdentificationDocumentTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.MaritalStatuses()
    {
        AddToLoad(async (service, ct) => MaritalStatuses = await service.MaritalStatuses(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.DrawingTypes()
    {
        AddToLoad(async (service, ct) => DrawingTypes = await service.DrawingTypes(ct));

        return this;
    }   
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.DrawingDurations()
    {
        AddToLoad(async (service, ct) => DrawingDurations = await service.DrawingDurations(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.SalesArrangementStates()
    {
        AddToLoad(async (service, ct) => SalesArrangementStates = await service.SalesArrangementStates(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.SalesArrangementTypes()
    {
        AddToLoad(async (service, ct) => SalesArrangementTypes = await service.SalesArrangementTypes(ct));

        return this;
    }    
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.Genders()
    {
        AddToLoad(async (service, ct) => Genders = await service.Genders(ct));

        return this;
    }   
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.EmploymentTypes()
    {
        AddToLoad(async (service, ct) => EmploymentTypes = await service.EmploymentTypes(ct));

        return this;
    }  
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.ObligationTypes()
    {
        AddToLoad(async (service, ct) => ObligationTypes = await service.ObligationTypes(ct));

        return this;
    }  
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.LegalCapacityRestrictionTypes()
    {
        AddToLoad(async (service, ct) => LegalCapacityRestrictionTypes = await service.LegalCapacityRestrictionTypes(ct));

        return this;
    }
    
    ICodebookManagerConfigurator ICodebookManagerConfigurator.DocumentTypes()
    {
        AddToLoad(async (service, ct) => DocumentTypes = await service.DocumentTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.EducationLevels()
    {
        AddToLoad(async (service, ct) => EducationLevels = await service.EducationLevels(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.SignatureTypes()
    {
        AddToLoad(async (service, ct) => SignatureTypes = await service.SignatureTypes(ct));

        return this;
    }

    ICodebookManagerConfigurator ICodebookManagerConfigurator.TinNoFillReasonsByCountry()
    {
        AddToLoad(async (service, ct) => TinNoFillReasonsByCountry = await service.TinNoFillReasonsByCountry(ct));

        return this;
    }

    private void AddToLoad(Func<ICodebookServiceClient, CancellationToken, Task> loader, [CallerMemberName] string callerName = "")
    {
        _codebooksToLoad.TryAdd(callerName, loader);
    }
}