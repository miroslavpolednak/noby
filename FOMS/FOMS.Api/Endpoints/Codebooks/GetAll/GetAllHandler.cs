﻿using DomainServices.CodebookService.Abstraction;
using FOMS.Api.Endpoints.Codebooks.GetAll.CodebookMap;

namespace FOMS.Api.Endpoints.Codebooks.GetAll;

internal class GetAllHandler
    : IRequestHandler<GetAllRequest, List<GetAllResponseItem>>
{
    public async Task<List<GetAllResponseItem>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        List<GetAllResponseItem> model = new();

        _logger.CodebooksGetAllStarted(request.CodebookCodes);

        foreach (var (original, key) in request.CodebookCodes)
        {
            var objects = await _codebookMap[key].GetObjects(_codebooks, cancellationToken);

            model.Add(new GetAllResponseItem(original, objects));
        }

        //foreach (var code in request.CodebookCodes)
        //    model.Add(await fillCodebook(code.Key, code.Original, cancellationToken));
        
        return model;
    }

    //TODO nejak automatizovat? Zase to nechci zpomalovat reflexi.... code generators?
    private async Task<GetAllResponseItem> fillCodebook(string code, string original, CancellationToken cancellationToken)
        => code switch
        {
            "academicdegreesafter" => new(original, (await _codebooks.AcademicDegreesAfter(cancellationToken))),
            "academicdegreesbefore" => new(original, (await _codebooks.AcademicDegreesBefore(cancellationToken))),
            "actioncodessavings" => new(original, (await _codebooks.ActionCodesSavings(cancellationToken)).Where(t => t.IsValid)),
            "actioncodessavingsloan" => new(original, (await _codebooks.ActionCodesSavingsLoan(cancellationToken)).Where(t => t.IsValid)),
            "bankcodes" => new(original, await _codebooks.BankCodes(cancellationToken)),
            "casestates" => new(original, await _codebooks.CaseStates(cancellationToken)),
            "classificationofeconomicactivities" => new(original, await _codebooks.ClassificationOfEconomicActivities(cancellationToken)),
            "contacttypes" => new(original, await _codebooks.ContactTypes(cancellationToken)),
            "countries" => new(original, (await _codebooks.Countries(cancellationToken)).OrderByDescending(t => t.IsDefault).ToList()),
            "currencies" => new(original, await _codebooks.Currencies(cancellationToken)),
            "customerprofiles" => new(original, await _codebooks.CustomerProfiles(cancellationToken)),
            "customerroles" => new(original, await _codebooks.CustomerRoles(cancellationToken)),
            "developers" => new(original, await _codebooks.Developers(cancellationToken)),
            "developerprojects" => new(original, await _codebooks.DeveloperProjects(cancellationToken)),
            "drawingdurations" => new(original, await _codebooks.DrawingDurations(cancellationToken)),
            "educationlevels" => new(original, await _codebooks.EducationLevels(cancellationToken)),
            "employmenttypes" => new(original, await _codebooks.EmploymentTypes(cancellationToken)),
            "fees" => new(original, await _codebooks.Fees(cancellationToken)),
            "fixedrateperiods" => new(original, await _codebooks.FixedRatePeriods(cancellationToken)),
            "genders" => new(original, await _codebooks.Genders(cancellationToken)),
            "householdtypes" => new(original, await _codebooks.HouseholdTypes(cancellationToken)),
            "identificationdocumenttypes" => new(original, await _codebooks.IdentificationDocumentTypes(cancellationToken)),
            "incomemaintypes" => new(original, await _codebooks.IncomeMainTypes(cancellationToken)),
            "incomeforeigntypes" => new(original, await _codebooks.IncomeForeignTypes(cancellationToken)),
            "Incomeothertypes" => new(original, await _codebooks.IncomeOtherTypes(cancellationToken)),
            "jobtypes" => new(original, await _codebooks.JobTypes(cancellationToken)),
            "legalcapacities" => new(original, await _codebooks.LegalCapacities(cancellationToken)),
            "loanpurposes" => new(original, (await _codebooks.LoanPurposes(cancellationToken)).Where(t => t.IsValid)),
            "loankinds" => new (original, (await _codebooks.LoanKinds(cancellationToken)).Where(t => t.IsValid)),
            "loaninterestrateannouncedtypes" => new(original, await _codebooks.LoanInterestRateAnnouncedTypes(cancellationToken)),
            "mandants" => new(original, await _codebooks.Mandants(cancellationToken)),
            "maritalstatuses" => new(original, await _codebooks.MaritalStatuses(cancellationToken)),
            "obligationcorrectiontypes" => new(original, await _codebooks.ObligationCorrectionTypes(cancellationToken)),
            "obligationtypes" => new(original, await _codebooks.ObligationTypes(cancellationToken)),
            "paymentdays" => new(original, (await _codebooks.PaymentDays(cancellationToken)).Where(t => t.ShowOnPortal).ToList()),
            "postcodes" => new (original, await _codebooks.PostCodes(cancellationToken)),
            "producttypes" => new(original, await getProductTypes(cancellationToken)),
            "propertysettlements" => new(original, await _codebooks.PropertySettlements(cancellationToken)),
            "salesarrangementstates" => new(original, (await _codebooks.SalesArrangementStates(cancellationToken)).Where(t => t.Id > 0).ToList()),
            "salesarrangementtypes" => new(original, await _codebooks.SalesArrangementTypes(cancellationToken)),
            "signaturetypes" => new(original, (await _codebooks.SignatureTypes(cancellationToken)).Where(t => t.Id > 0).ToList()),
            "realestatetypes" => new(original, (await _codebooks.RealEstateTypes(cancellationToken)).Where(t => t.Id > 0).ToList()),
            "realestatepurchasetypes" => new(original, await _codebooks.RealEstatePurchaseTypes(cancellationToken)),
            "workflowtaskcategories" => new(original, await _codebooks.WorkflowTaskCategories(cancellationToken)),
            "workflowtaskstates" => new(original, await _codebooks.WorkflowTaskStates(cancellationToken)),
            "workflowtasktypes" => new(original, await _codebooks.WorkflowTaskTypes(cancellationToken)),
            "worksectors" => new(original, await _codebooks.WorkSectors(cancellationToken)),


            //"marketingactions" => new(original, await _codebooks.MarketingActions(cancellationToken)),

            //"residencytypes" => new(original, await _codebooks.ResidencyTypes()),//!!!
            //"mktactioncodessavings" => new(original, (await _codebooks.MktActionCodesSavings())),//!!!
            //"nationalities" => new(original, await _codebooks.Nationalities()),//!!!
            //"persondegreeafter" => new(original, await _codebooks.PersonDegreeAfter()),//!!!
            //"persondegreebefore" => new(original, await _codebooks.PersonDegreeBefore()),//!!!

            _ => throw new NotImplementedException($"Codebook code '{original}' is not implemented")
        };

    private async Task<List<Dto.ProductTypeItem>> getProductTypes(CancellationToken cancellationToken)
    {
        var loankinds = await _codebooks.LoanKinds(cancellationToken);
        var codeboook = await _codebooks.ProductTypes(cancellationToken);

        return codeboook.Select(t => new Dto.ProductTypeItem
        {
            Id = t.Id,
            LoanAmountMax = t.LoanAmountMax,
            LoanAmountMin = t.LoanAmountMin,
            LoanDurationMax = t.LoanDurationMax,
            LoanDurationMin = t.LoanDurationMin,
            LtvMax = t.LtvMax,
            LtvMin = t.LtvMin,
            MandantId = t.MandantId,
            Name = t.Name,
            LoanKinds = t.LoanKindIds is null ? null : loankinds.Where(x => t.LoanKindIds.Contains(x.Id)).ToList()
        }).ToList();
    }

    private readonly ICodebookServiceAbstraction _codebooks;
    private readonly ICodebookMap _codebookMap;
    private readonly ILogger<GetAllHandler> _logger;

    public GetAllHandler(ICodebookServiceAbstraction codebooks, ICodebookMap codebookMap, ILogger<GetAllHandler> logger)
    {
        _logger = logger;
        _codebooks = codebooks;
        _codebookMap = codebookMap;
    }
}
