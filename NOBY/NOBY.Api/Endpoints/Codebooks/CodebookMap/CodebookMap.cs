using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;
using NOBY.Api.Endpoints.Codebooks.GetAll.Dto;
using Codebook = DomainServices.CodebookService.Contracts.Endpoints;

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
            if (!_endpoints.ContainsKey(code))
                throw new NotImplementedException($"Codebook code '{code}' is not implemented");

            return _endpoints[code]; 

        }
    }

    private void ConfigureMap()
    {
        AddCodebook(s => s.AcademicDegreesAfter);
        AddCodebook(s => s.AcademicDegreesBefore);
        AddCodebook(s => s.ActionCodesSavings, c => c.Cast<GenericCodebookItem>().Where(t => t.IsValid));
        AddCodebook(s => s.ActionCodesSavingsLoan, c => c.Cast<GenericCodebookItem>().Where(t => t.IsValid));
        AddCodebook(s => s.BankCodes);
        AddCodebook(s => s.CaseStates);
        AddCodebook(s => s.ClassificationOfEconomicActivities);
        AddCodebook(s => s.ContactTypes);
        AddCodebook(s => s.Countries, c => c.Cast<Codebook.Countries.CountriesItem>().OrderByDescending(t => t.IsDefault));
        AddCodebook(s => s.Currencies);
        AddCodebook(s => s.CustomerProfiles);
        AddCodebook(s => s.CustomerRoles);
        AddCodebook(s => s.Developers);
        AddCodebook(s => s.DeveloperProjects);
        AddCodebook(s => s.DocumentOnSATypes);
        AddCodebook(s => s.DrawingDurations);
        AddCodebook(s => s.DrawingTypes, c => c.Cast<Codebook.DrawingTypes.DrawingTypeItem>().Where(t => t.Id > 0));
        AddCodebook(s => s.EaCodesMain, c => c.Cast<Codebook.EaCodesMain.EaCodeMainItem>().Where(t => t.IsValid));
        AddCodebook(s => s.EducationLevels);
        AddCodebook(s => s.EmploymentTypes);
        AddCodebook(s => s.Fees);
        AddCodebook(s => s.FixedRatePeriods);
        AddCodebook(s => s.Genders);
        AddCodebook(s => s.HouseholdTypes);
        AddCodebook(s => s.IdentificationDocumentTypes);
        AddCodebook(s => s.IncomeMainTypes);
        AddCodebook(s => s.IncomeForeignTypes);
        AddCodebook(s => s.IncomeOtherTypes);
        AddCodebook(s => s.JobTypes);
        AddCodebook(s => s.LegalCapacityRestrictionTypes);
        AddCodebook(s => s.LoanPurposes, c => c.Cast<Codebook.LoanPurposes.LoanPurposesItem>().Where(t => t.IsValid));
        AddCodebook(s => s.LoanKinds, c => c.Cast<Codebook.LoanKinds.LoanKindsItem>().Where(t => t.IsValid));
        AddCodebook(s => s.LoanInterestRateAnnouncedTypes);
        AddCodebook(s => s.Mandants);
        AddCodebook(s => s.MaritalStatuses);
        AddCodebook(s => s.ObligationCorrectionTypes);
        AddCodebook(s => s.ObligationTypes);
        AddCodebook(s => s.FormTypes, c => c.Cast<Codebook.FormTypes.FormTypeItem>().Where(t => t.IsValid));
        AddCodebook(s => s.PaymentDays, c => c.Cast<Codebook.PaymentDays.PaymentDayItem>().Where(t => t.ShowOnPortal));
        AddCodebook(s => s.PayoutTypes);
        AddCodebook(s => s.PostCodes);
        AddCodebook(s => s.PropertySettlements);
        AddCodebook(s => s.SalesArrangementStates, c => c.Cast<Codebook.SalesArrangementStates.SalesArrangementStateItem>().Where(t => t.Id > 0));
        AddCodebook(s => s.SalesArrangementTypes);
        AddCodebook(s => s.SignatureTypes, c => c.Cast<Codebook.SignatureTypes.SignatureTypeItem>().Where(t => t.Id > 0));
        AddCodebook(s => s.StatementTypes);
        AddCodebook(s => s.RealEstateTypes, c => c.Cast<Codebook.RealEstateTypes.RealEstateTypeItem>().Where(t => t.Id > 0));
        AddCodebook(s => s.RealEstatePurchaseTypes);
        AddCodebook(s => s.WorkflowTaskCategories);
        AddCodebook(s => s.WorkflowTaskStates);
        AddCodebook(s => s.WorkflowTaskTypes);
        AddCodebook(s => s.WorkSectors);

        var productTypesEndpoint = new CodebookEndpoint<ProductTypeItem>("producttypes", ProductTypesCall, default!);
        _endpoints.Add(productTypesEndpoint.Code, productTypesEndpoint);
    }

    private void AddCodebook(Expression<Func<ICodebookServiceClients, Delegate>> expression, Func<IEnumerable<object>, IEnumerable<object>> customizeResult = default!)
    {
        var methodName = GetMethodName((UnaryExpression)expression.Body);
        var returnType = GetCodebookReturnType(expression.Body);

        var endpointGenericType = typeof(CodebookEndpoint<>).MakeGenericType(returnType);

        var endpoint = (ICodebookEndpoint)Activator.CreateInstance(endpointGenericType, methodName, expression.Compile(), customizeResult)!;

        _endpoints.Add(methodName, endpoint);
    }

    private static string GetMethodName(UnaryExpression body)
    {
        var methodCallExpression = (MethodCallExpression)body.Operand;
        var constant = (ConstantExpression)methodCallExpression.Object!;

        var methodInfo = (MethodInfo)constant.Value!;

        return methodInfo.Name.ToLowerInvariant();
    }

    private static Type GetCodebookReturnType(Expression body)
    {
        var returnTaskType = body.Type.GetGenericArguments().Last();
        var returnTaskListType = returnTaskType.GetGenericArguments().First();

        return returnTaskListType.GetGenericArguments().First();
    }

    private static Func<CancellationToken, Task<List<ProductTypeItem>>> ProductTypesCall(ICodebookServiceClients codebookService)
    {
        return CallProductTypesEndpoint;

        async Task<List<ProductTypeItem>> CallProductTypesEndpoint(CancellationToken cancellationToken)
        {
            var loanKinds = await codebookService.LoanKinds(cancellationToken);
            var productTypes = await codebookService.ProductTypes(cancellationToken);

            return productTypes
                .Where(t => t.IsValid)
                .Select(t => new GetAll.Dto.ProductTypeItem
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
                    LoanKinds = t.LoanKindIds is null ? null : loanKinds.Where(x => t.LoanKindIds.Contains(x.Id)).ToList()
                })
                .ToList();
        }
    }

    public IEnumerator<ICodebookEndpoint> GetEnumerator() => _endpoints.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}