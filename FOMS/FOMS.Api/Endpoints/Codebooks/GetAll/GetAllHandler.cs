﻿using DomainServices.CodebookService.Abstraction;

namespace FOMS.Api.Endpoints.Codebooks.GetAll;

internal class GetAllHandler
    : IRequestHandler<GetAllRequest, List<GetAllResponseItem>>
{
    public async Task<List<GetAllResponseItem>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        List<GetAllResponseItem> model = new();

        _logger.CodebooksGetAllStarted(request.CodebookCodes);

        foreach (var code in request.CodebookCodes)
            model.Add(await fillCodebook(code.Key, code.Original, cancellationToken));
        
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
            "casestates" => new(original, await _codebooks.CaseStates(cancellationToken)),
            "classificationofeconomicactivities" => new(original, await _codebooks.ClassificationOfEconomicActivities(cancellationToken)),
            "contacttypes" => new(original, await _codebooks.ContactTypes(cancellationToken)),
            "countries" => new(original, (await _codebooks.Countries(cancellationToken)).OrderByDescending(t => t.IsDefault).ToList()),
            "currencies" => new(original, await _codebooks.Currencies(cancellationToken)),
            "customerroles" => new(original, await _codebooks.CustomerRoles(cancellationToken)),
            "developers" => new(original, await _codebooks.Developers(cancellationToken)),
            "developerprojects" => new(original, await _codebooks.DeveloperProjects(cancellationToken)),
            "educationlevels" => new(original, await _codebooks.EducationLevels(cancellationToken)),
            "employmenttypes" => new(original, await _codebooks.EmploymentTypes(cancellationToken)),
            "fees" => new(original, await _codebooks.Fees(cancellationToken)),
            "fixedrateperiods" => new(original, await _codebooks.FixedRatePeriods(cancellationToken)),
            "genders" => new(original, await _codebooks.Genders(cancellationToken)),
            "identificationdocumenttypes" => new(original, await _codebooks.IdentificationDocumentTypes(cancellationToken)),
            "incomemaintypes" => new(original, await _codebooks.IncomeMainTypes(cancellationToken)),
            "incomeforeigntypes" => new(original, await _codebooks.IncomeForeignTypes(cancellationToken)),
            "Incomeothertypes" => new(original, await _codebooks.IncomeOtherTypes(cancellationToken)),
            "jobtypes" => new(original, await _codebooks.JobTypes(cancellationToken)),
            "legalcapacities" => new(original, await _codebooks.LegalCapacities(cancellationToken)),
            "loanpurposes" => new(original, (await _codebooks.LoanPurposes(cancellationToken)).Where(t => t.IsValid)),
            "loankinds" => new(original, (await _codebooks.LoanKinds(cancellationToken)).Where(t => t.IsValid)),
            "mandants" => new(original, await _codebooks.Mandants(cancellationToken)),
            "maritalstatuses" => new(original, (await _codebooks.MaritalStatuses(cancellationToken))),
            "obligationtypes" => new(original, await _codebooks.ObligationTypes(cancellationToken)),
            "paymentdays" => new(original, (await _codebooks.PaymentDays(cancellationToken)).Where(t => t.ShowOnPortal).ToList()),
            "postcodes" => new (original, await _codebooks.PostCodes(cancellationToken)),
            "producttypes" => new(original, await _codebooks.ProductTypes(cancellationToken)),
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

            //"residencytypes" => new(original, await _codebooks.ResidencyTypes()),//!!!
            //"mktactioncodessavings" => new(original, (await _codebooks.MktActionCodesSavings())),//!!!
            //"nationalities" => new(original, await _codebooks.Nationalities()),//!!!
            //"persondegreeafter" => new(original, await _codebooks.PersonDegreeAfter()),//!!!
            //"persondegreebefore" => new(original, await _codebooks.PersonDegreeBefore()),//!!!

            _ => throw new NotImplementedException($"Codebook code '{original}' is not implemented")
        };

    private readonly ICodebookServiceAbstraction _codebooks;
    private readonly ILogger<GetAllHandler> _logger;

    public GetAllHandler(ICodebookServiceAbstraction codebooks, ILogger<GetAllHandler> logger)
    {
        _logger = logger;
        _codebooks = codebooks;
    }
}
