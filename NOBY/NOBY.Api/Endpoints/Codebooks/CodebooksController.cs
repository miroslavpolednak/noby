using Asp.Versioning;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Codebooks;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/codebooks")]
public class CodebooksController(IMediator _mediator) : ControllerBase
{
    /// <summary>Kolekce vyžadaných číselniků.</summary>
    /// <remarks>
    /// Vrací číselníky identifikované query parametrem "q". Jednotlivé číselniky jsou oddělené čárkou.<br/>
    /// Aktuálně implementované číselniky jsou:
    ///
    /// Na stránkách CFL (níže jména číselníků - odkazy na číselníky do CFL) je uvedeno u sloupců <b>(není v FE API)</b>, pokud nejsou na výstupu FE API.
    ///
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421388420">AcademicDegreesAfter</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421388414">AcademicDegreesBefore</a>
    /// - <a href="https://wiki.kb.cz/display/HT/AcvAttachmentCategory">AcvAttachmentCategories</a>
    /// - ActionCodesSavings - obsolete
    /// - ActionCodesSavingsLoan - obsolete
    /// - <a href="https://wiki.kb.cz/confluence/display/HT/CaseState">CaseStates</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216251">ClassificationOfEconomicActivities</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=392877756">ContactTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=405522415">Countries</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=483360846">CountryCodePhoneIdc</a>
    /// - <a href="https://wiki.kb.cz/display/HT/CovenantType">CovenantTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=424127746">Currencies</a>
    /// - <a href="https://wiki.kb.cz/display/HT/CustomerProfile">CustomerProfiles</a> 
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421386916">CustomerRoles</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413658556">AddressTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=455007953">BankCodes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046695">Developers [Deprecated]</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046776">DeveloperProjects [Deprecated]</a>
    /// - <a href="https://wiki.kb.cz/display/HT/DocumentOnSAType">DocumentOnSATypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/DocumentTemplateVersion">DocumentTemplateVersions</a>
    /// - <a href="https://wiki.kb.cz/display/HT/DocumentTemplateVariant">DocumentTemplateVariants</a>
    /// - <a href="https://wiki.kb.cz/display/HT/DocumentType">DocumentTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=450580207">DrawingDurations</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=444604999">DrawingTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=400823251">EaCodesMain</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=419562802">EducationLevels</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216233">EmploymentTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=835723767">ExtraPaymentReasons</a>
    /// - <a href="https://wiki.kb.cz/display/HT/ExtraPaymentType">ExtraPaymentTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438049777">Fees</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=800055209">FeeChangeRequests</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413645701">FixedRatePeriods</a>
    /// - <a href="">FlowSwitchStates</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=405524365">Genders</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=857276813">HandoverTypeDetails</a>
    /// - <a href="https://wiki.kb.cz/display/HT/HouseholdType+-+MOCK">HouseholdTypes</a> 
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=392871926">IdentificationDocumentTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/IdentificationSubjectMethod+%28CB_IdentificationMethodType%29+-+MOCK">IdentificationSubjectMethods</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=426158089">IncomeMainTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=476967686">IncomeMainTypesAML</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216188">IncomeForeignTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440881532">IncomeOtherTypes</a> 
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216289">JobTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=424118208">LegalCapacityRestrictionTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413109663">LoanPurposes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413108848">LoanKinds</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413108848">LoanInterestRateAnnouncedTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/Mandant">Mandants</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=417279748">MaritalStatuses</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=417279748">MarketingActions</a> - zatím není na FE API implementováno
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=476967710">NetMonthEarnings</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440890324">ObligationCorrectionTypes</a> 
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=426150084">ObligationTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=409432393">FormTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=438046178">PaymentDays</a>
    /// - <a href="https://wiki.kb.cz/display/HT/PayoutType">PayoutTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=421389753">PostCodes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=392883923">ProductTypes</a> 
    /// - <a href="https://wiki.kb.cz/display/HT/ProfessionCategory+%28CB_Prof_Cat1%29+-+MOCK">ProfessionCategories</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=476967580">ProfessionTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413648025">PropertySettlements</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=664578092">RealEstateValuationLocalSurveyFunctions</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RealEstateState">RealEstateStates</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RealEstateSubtype">RealEstateSubtypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413632253">RealEstateTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413632953">RealEstatePurchaseTypes</a>
    /// - <a href="https://wiki.kb.cz/x/rZ0hMg">RefixationDocumentTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RefixationOfferType">RefixationOfferTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RefinancingState">RefinancingStates</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RefinancingType">RefinancingTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=835727392">ResponseCodeTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/SalesArrangementState">SalesArrangementStates</a>
    /// - <a href="https://wiki.kb.cz/display/HT/SalesArrangementType">SalesArrangementTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/SignatureType">SignatureTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=800899040">SignatureTypeDetails</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=589235799">StatementFrequencies</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RealEstateValuationFlatSchema">RealEstateValuationFlatSchemas</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RealEstateValuationBuildingMaterialStructure">RealEstateValuationBuildingMaterialStructures</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RealEstateValuationBuildingAge">RealEstateValuationBuildingAges</a>
    /// - <a href="https://wiki.kb.cz/display/HT/RealEstateValuationBuildingTechnicalState">RealEstateValuationBuildingTechnicalStates</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=664578092">RealEstateValuationLocalSurveyFunctions</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=589235789">StatementSubscriptionTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=417284324">StatementTypes</a>
    /// - <a href="https://wiki.kb.cz/display/HT/TinFormatByCountry+%28CB_CmTrTinFormat%29+-+MOCK">TinFormatsByCountry</a>
    /// - <a href="https://wiki.kb.cz/display/HT/TinNoFillReasonsByCountry+%28CB_CmTrTinCountry%29+-+MOCK">TinNoFillReasonsByCountry</a>
    /// - <a href="https://wiki.kb.cz/display/HT/WorkflowTaskStateNoby">WorkflowTaskStatesNoby</a>
    /// - <a href="https://wiki.kb.cz/display/HT/WorkflowTaskCategory">WorkflowTaskCategories</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=549782919">WorkflowTaskSigningResponseTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440871662">WorkflowTaskStates</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440879561">WorkflowTaskTypes</a>
    /// - <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=430216261">WorkSectors</a>
    ///
    /// Struktura číselníku který není k nalezení dle jména ve schematech response je schema GenericCodebookItem.
    ///
    /// </remarks>
    /// <param name="codebookTypes">Kody pozadovanych ciselniku oddelene carkou. Nazvy NEjsou case-sensitive. Example: q=productTypes,actionCodesSavings</param>
    /// <returns>Kolekce vyzadanych ciselniku.</returns>
    [HttpGet("get-all")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<CodebooksGetAllResponseItem>), StatusCodes.Status200OK)]
    public async Task<List<CodebooksGetAllResponseItem>> GetAll([FromQuery(Name = "q")] string codebookTypes, CancellationToken cancellationToken)
        => await _mediator.Send(new GetAll.GetAllRequest(codebookTypes), cancellationToken);

    /// <summary>
    /// Vraci seznam podporovanych codebooks na FE API
    /// </summary>
    [HttpGet("supported")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<CodebooksSupportedCodebook>), StatusCodes.Status200OK)]
    public List<CodebooksSupportedCodebook> GetSupported([FromServices] CodebookMap.ICodebookMap codebookMap)
    {
        return codebookMap.Select(m => new CodebooksSupportedCodebook
        {
            Name = m.Code,
            Type = m.ReturnType.Name
        }).ToList();
    }

    /// <summary>
    /// Číselník druhu úveru.
    /// </summary>
    /// <param name="productTypeId">ID typu produktu, pro který se mají vrátit druhy úvěru.</param>
    [HttpGet("product-loan-kinds")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<ShareTypesGenericCodebookItem>), StatusCodes.Status200OK)]
    public async Task<List<ShareTypesGenericCodebookItem>?> GetProductLoanKinds([FromQuery] int productTypeId, [FromServices] ICodebookServiceClient svc, CancellationToken cancellationToken)
    {
        var loanKindsIds = (await svc.ProductTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == productTypeId && t.IsValid.GetValueOrDefault())?
            .LoanKindIds
            .ToList();
        if (loanKindsIds is null) return null;

        return (await svc.LoanKinds(cancellationToken))
            .Where(t => loanKindsIds.Contains(t.Id))
            .Select(t => new ShareTypesGenericCodebookItem
            {
                Id = t.Id,
                Code = t.Code,
                Description = t.Description,
                IsDefault = t.IsDefault,
                IsValid = t.IsValid,
                Name = t.Name,
                Order = t.Order,
                MandantId = t.MandantId
            })
            .ToList();
    }

    /// <summary>
    /// Vrací seznam všedních dnů, které jsou svátky
    /// </summary>
    [HttpPost("banking-days")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<DateOnly>), StatusCodes.Status200OK)]
    public async Task<List<DateOnly>> GetNonBankingDays([FromBody] CodebooksGetNonBankingDaysRequest request, [FromServices] ICodebookServiceClient svc, CancellationToken cancellationToken)
        => (await svc.GetNonBankingDays(request.DateFrom, request.DateTo, cancellationToken)).ToList();

    /// <summary>
    /// FixedRatePeriod s filtrací na product
    /// </summary>
    /// <remarks>
    /// Vyfiltruje fixed rate dle zadaného ProductTypeId a pouze s atributem IsNewProduct=true.
    /// </remarks>
    /// <param name="productTypeId">ID typu produktu</param>
    [HttpGet("fixed-rate-periods")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<CodebooksFixedRatePeriodItem>), StatusCodes.Status200OK)]
    public async Task<List<CodebooksFixedRatePeriodItem>?> GetFixedRatePeriods([FromQuery] int productTypeId, [FromServices] ICodebookServiceClient svc, CancellationToken cancellationToken)
        => (await svc.FixedRatePeriods(cancellationToken))
            .Where(t => t.ProductTypeId == productTypeId && t.IsNewProduct)
            .Select(t => new CodebooksFixedRatePeriodItem
            {
                FixedRatePeriod = t.FixedRatePeriod,
                InterestRateAlgorithm = t.InterestRateAlgorithm,
                IsNewProduct = t.IsNewProduct,
                IsValid = t.IsValid,
                MandantId = t.MandantId,
                ProductTypeId = productTypeId,
            })
            .ToList();

    /// <summary>
    /// Detail developera
    /// </summary>
    /// <remarks>
    /// Vrátí detail developera dle developerId na vstupu.
    /// </remarks>
    /// <param name="developerId">ID developera</param>
    [HttpGet("developer/{developerId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CodebooksDeveloper), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=C719D03C-9DF1-4ffc-AFAC-ED79AB01CC34")]
    public async Task<CodebooksDeveloper> GetDeveloper([FromRoute] int developerId, [FromServices] ICodebookServiceClient svc, CancellationToken cancellationToken)
    {
        var developer = await svc.GetDeveloper(developerId, cancellationToken);
        return new CodebooksDeveloper
        {
            Name = developer.Name,
            Cin = developer.Cin,
            Status = new()
            {
                StatusId = developer.StatusId,
                StatusText = developer.StatusText,
            },
            ShowBenefitsPackage = developer.BenefitPackage && developer.IsBenefitValid,
            ShowBenefitsBeyondPackage = developer.BenefitsBeyondPackage && developer.IsBenefitValid
        };
    }

    /// <summary>
    /// Detail developeského projektu
    /// </summary>
    /// <remarks>
    /// Vrátí detail developerského projektu dle developerProjectId na vstupu.
    /// </remarks>
    /// <param name="developerId">ID developera</param>
    [HttpGet("developer/{developerId:int}/developer-project/{developerProjectId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CodebooksGetDeveloperProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=9429D814-AAFA-42df-8782-DFF85B96CFDB")]
    public async Task<CodebooksGetDeveloperProjectResponse> GetDeveloperProject([FromRoute] int developerId, [FromRoute] int developerProjectId, [FromServices] ICodebookServiceClient svc, CancellationToken cancellationToken)
    {
        var result = await svc.GetDeveloperProject(developerId, developerProjectId, cancellationToken);

        return new CodebooksGetDeveloperProjectResponse
        {
            Id = result.Id,
            IsValid = result.IsValid,
            MassEvaluation = result.MassEvaluation,
            MassEvaluationText = result.MassEvaluationText,
            Name = result.Name,
            Place = result.Place,
            Recommandation = result.Recommandation,
            WarningForKb = result.WarningForKb,
            WarningForMp = result.WarningForMp,
            Web = result.Web
        };
    }
}