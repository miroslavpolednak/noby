using System.Collections;
using System.Linq.Expressions;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.Codebooks.CodebookMap;

public class CodebookMap : ICodebookMap
{
    private readonly Dictionary<string, ICodebookEndpoint> _endpoints = new(20);

    public CodebookMap()
    {
        ConfigureMap();
    }

    public ICodebookEndpoint this[string code]
    {
        get
        {
            var optimizedCode = code.ToLowerInvariant();

            if (!_endpoints.ContainsKey(optimizedCode))
                throw new NotImplementedException($"Codebook code '{code}' is not implemented");

            return _endpoints[optimizedCode]; 

        }
    }

    private void ConfigureMap()
    {
        AddCodebook((s, ct) => s.AcademicDegreesAfter(ct));
        AddCodebook((s, ct) => s.AcademicDegreesBefore(ct));
        AddCodebook((s, ct) => s.AddressTypes(ct));
        AddCodebook((s, ct) => s.AcvAttachmentCategories(ct));
        AddCodebook((s, ct) => s.BankCodes(ct));
        AddCodebook((s, ct) => s.CaseStates(ct));
        AddCodebook((s, ct) => s.ClassificationOfEconomicActivities(ct));
        AddCodebook((s, ct) => s.ContactTypes(ct));
        AddCodebook((s, ct) => s.Countries(ct), c => c.Cast<CountriesResponse.Types.CountryItem>().OrderByDescending(t => t.IsDefault));
        AddCodebook((s, ct) => s.CountryCodePhoneIdc(ct));
        AddCodebook((s, ct) => s.CovenantTypes(ct));
        AddCodebook((s, ct) => s.Currencies(ct));
        AddCodebook((s, ct) => s.CustomerProfiles(ct));
        AddCodebook((s, ct) => s.CustomerRoles(ct));
        AddCodebook((s, ct) => s.DocumentOnSATypes(ct));
        AddCodebook((s, ct) => s.DocumentTemplateVersions(ct));
        AddCodebook((s, ct) => s.DocumentTemplateVariants(ct));
        AddCodebook((s, ct) => s.DocumentTypes(ct));
        AddCodebook((s, ct) => s.DrawingDurations(ct));
        AddCodebook((s, ct) => s.DrawingTypes(ct), c => c.Cast<DrawingTypesResponse.Types.DrawingTypeItem>().Where(t => t.Id > 0));
        AddCodebook((s, ct) => s.EaCodesMain(ct));
        AddCodebook((s, ct) => s.EducationLevels(ct));
        AddCodebook((s, ct) => s.EmploymentTypes(ct));
        AddCodebook((s, ct) => s.Fees(ct));
        AddCodebook((s, ct) => s.FixedRatePeriods(ct));
        AddCodebook((s, ct) => s.Genders(ct));
        AddCodebook((s, ct) => s.HouseholdTypes(ct));
        AddCodebook((s, ct) => s.IdentificationDocumentTypes(ct));
        AddCodebook((s, ct) => s.IdentificationSubjectMethods(ct));
        AddCodebook((s, ct) => s.IncomeMainTypes(ct));
        AddCodebook((s, ct) => s.IncomeForeignTypes(ct));
        AddCodebook((s, ct) => s.IncomeOtherTypes(ct));
        AddCodebook((s, ct) => s.JobTypes(ct));
        AddCodebook((s, ct) => s.LegalCapacityRestrictionTypes(ct));
        AddCodebook((s, ct) => s.LoanPurposes(ct));
        AddCodebook((s, ct) => s.LoanKinds(ct));
        AddCodebook((s, ct) => s.LoanInterestRateAnnouncedTypes(ct));
        AddCodebook((s, ct) => s.Mandants(ct));
        AddCodebook((s, ct) => s.MaritalStatuses(ct));
        AddCodebook((s, ct) => s.NetMonthEarnings(ct));
        AddCodebook((s, ct) => s.ObligationCorrectionTypes(ct));
        AddCodebook((s, ct) => s.ObligationTypes(ct));
        AddCodebook((s, ct) => s.FormTypes(ct));
        AddCodebook((s, ct) => s.PaymentDays(ct), c => c.Cast<PaymentDaysResponse.Types.PaymentDayItem>().Where(t => t.ShowOnPortal));
        AddCodebook((s, ct) => s.PayoutTypes(ct));
        AddCodebook((s, ct) => s.PostCodes(ct));
        AddCodebook((s, ct) => s.ProfessionCategories(ct), c => c.Cast<ProfessionCategoriesResponse.Types.ProfessionCategoryItem>());
        AddCodebook((s, ct) => s.ProfessionTypes(ct));
        AddCodebook((s, ct) => s.PropertySettlements(ct));
        AddCodebook((s, ct) => s.SalesArrangementStates(ct), c => c.Cast<SalesArrangementStatesResponse.Types.SalesArrangementStateItem>().Where(t => t.Id > 0));
        AddCodebook((s, ct) => s.SalesArrangementTypes(ct));
        AddCodebook((s, ct) => s.SignatureTypes(ct), c => c.Cast<GenericCodebookResponse.Types.GenericCodebookItem>().Where(t => t.Id > 0));
        AddCodebook((s, ct) => s.StatementTypes(ct));
        AddCodebook((s, ct) => s.TinFormatsByCountry(ct));
        AddCodebook((s, ct) => s.TinNoFillReasonsByCountry(ct));
        AddCodebook((s, ct) => s.RealEstateStates(ct), c => c.Cast<GenericCodebookResponse.Types.GenericCodebookItem>().Where(t => t.Id > 0));
        AddCodebook((s, ct) => s.RealEstateSubtypes(ct));
        AddCodebook((s, ct) => s.RealEstateTypes(ct), c => c.Cast<RealEstateTypesResponse.Types.RealEstateTypesResponseItem>().Where(t => t.Id > 0));
        AddCodebook((s, ct) => s.RealEstatePurchaseTypes(ct));
        AddCodebook((s, ct) => s.WorkflowTaskStatesNoby(ct));
        AddCodebook((s, ct) => s.WorkflowTaskCategories(ct));
        AddCodebook((s, ct) => s.WorkflowTaskSigningResponseTypes(ct));
        AddCodebook((s, ct) => s.WorkflowTaskStates(ct));
        AddCodebook((s, ct) => s.WorkflowTaskTypes(ct));
        AddCodebook((s, ct) => s.WorkSectors(ct));
        AddCodebook((s, ct) => s.ProductTypes(ct));
        AddCodebook((s, ct) => s.IncomeMainTypesAML(ct));
        AddCodebook((s, ct) => s.StatementSubscriptionTypes(ct));
        AddCodebook((s, ct) => s.StatementFrequencies(ct));
    }

    private void AddCodebook<TReturn>(Expression<Func<ICodebookServiceClient, CancellationToken, TReturn>> expression, Func<IEnumerable<object>, IEnumerable<object>> customizeResult = default!) where TReturn : Task
    {
        var methodName = ((MethodCallExpression)expression.Body).Method.Name;
        var returnType = GetCodebookReturnType(typeof(TReturn));

        var endpointGenericType = typeof(CodebookEndpoint<>).MakeGenericType(returnType);

        var endpoint = (ICodebookEndpoint)Activator.CreateInstance(endpointGenericType, methodName, expression.Compile(), customizeResult)!;

        _endpoints.Add(methodName.ToLowerInvariant(), endpoint);
    }

    private static Type GetCodebookReturnType(Type taskType)
    {
        var returnTaskType = taskType.GetGenericArguments().Last();

        return returnTaskType.GetGenericArguments().First();
    }

    public IEnumerator<ICodebookEndpoint> GetEnumerator() => _endpoints.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}