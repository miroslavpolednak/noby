using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;
using __SA = DomainServices.SalesArrangementService.Contracts;
using CIS.Core.Attributes;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

[SelfService, TransientService]
internal sealed class UpdateParametersValidator(
    ICodebookServiceClient _codebookService, 
    IProductServiceClient _productService, 
    Services.Validators.IBankAccountValidatorService _bankAccountValidator)
{
    public async Task<SalesArrangementSharedParametersMortgage?> Validate(SalesArrangementSharedParametersMortgage? parameters, __SA.SalesArrangement salesArrangement)
    {
        if (string.IsNullOrEmpty(parameters?.IncomeCurrencyCode) || string.IsNullOrEmpty(parameters?.ResidencyCurrencyCode))
        {
            throw new NobyValidationException(90019);
        }

        await validateLoanRealEstate(parameters.LoanRealEstates);
        return parameters;
    }

    public async Task<SalesArrangementUpdateParametersHubn?> Validate(SalesArrangementUpdateParametersHubn? parameters, __SA.SalesArrangement salesArrangement)
    {
        validateExtensionDrawingDate(parameters?.DrawingDateTo?.IsActive ?? false, parameters?.DrawingDateTo?.ExtensionDrawingDateToByMonths);
        await validateLoanRealEstate(parameters?.LoanRealEstates);
        await validateApplicant(parameters?.Applicant, salesArrangement.CaseId);
        return parameters;
    }

    public async Task<SalesArrangementSharedParametersDrawing?> Validate(SalesArrangementSharedParametersDrawing? parameters, __SA.SalesArrangement salesArrangement)
    {
        await validateApplicant(parameters?.Applicant, salesArrangement.CaseId);

        if (parameters?.PayoutList is null || parameters.PayoutList.Count == 0)
        {
            throw new NobyValidationException(90032);
        }

        if ((parameters?.Agent?.IsActive ?? false)
            && (
                string.IsNullOrEmpty(parameters.Agent.FirstName)
                || string.IsNullOrEmpty(parameters.Agent.LastName)
                || string.IsNullOrEmpty(parameters.Agent.IdentificationDocument?.Number)
                || parameters.Agent.IdentificationDocument?.IdentificationDocumentTypeId == null
                || !parameters.Agent.DateOfBirth.HasValue
                )
            )
        {
            throw new NobyValidationException(90032);
        }

        if ((parameters?.PayoutList?.Any(t => !_bankAccountValidator.IsBankAccountAndCodeValid(t)) ?? false)
            || !_bankAccountValidator.IsBankAccountAndCodeValid(parameters?.RepaymentAccount))
        {
            throw new NobyValidationException(90032, "Invalid bank account");
        }
        return parameters;
    }

    public async Task<SalesArrangementUpdateParametersGeneralChange?> Validate(SalesArrangementUpdateParametersGeneralChange? parameters, __SA.SalesArrangement salesArrangement)
    {
        validateExtensionDrawingDate(parameters?.DrawingDateTo.IsActive ?? false, parameters?.DrawingDateTo.ExtensionDrawingDateToByMonths);
        await validateApplicant(parameters?.Applicant, salesArrangement.CaseId);
        return parameters;
    }

    public async Task<SalesArrangementUpdateParametersCustomerChange?> Validate(SalesArrangementUpdateParametersCustomerChange? parameters, __SA.SalesArrangement salesArrangement)
    {
        foreach (var applicant in salesArrangement.CustomerChange.Applicants)
        {
            await validateApplicant(applicant.Identity.Select(identity => (SharedTypesCustomerIdentity)identity!).ToList(), salesArrangement.CaseId);
        }
        return parameters;
    }

    public Task<SalesArrangementUpdateParametersCustomerChange3602?> Validate(SalesArrangementUpdateParametersCustomerChange3602? parameters, __SA.SalesArrangement salesArrangement)
    {
        return Task.FromResult(parameters);
    }

    private static void validateExtensionDrawingDate(bool isActive, int? extension)
    {
        if (!isActive)
            return;

        if (extension.HasValue && (extension.Value < 1 || extension.Value > 36))
        {
            throw new NobyValidationException("extensionDrawingDateToByMonths must be between 1 and 36");
        }
    }

    private async Task validateApplicant(List<SharedTypesCustomerIdentity>? identities, long caseId)
    {
        if (!(identities?.Any() ?? false))
        {
            throw new NobyValidationException(90033);
        }

        var customers = await _productService.GetCustomersOnProduct(caseId);
        foreach (var customer in customers.Customers)
        {
            if (customer.CustomerIdentifiers.Any(t => identities.Any(x => (SharedTypesCustomerIdentityScheme)t.IdentityScheme == x.Scheme && t.IdentityId == x.Id)))
            {
                return;
            }
        }

        throw new NobyValidationException(90033);
    }

    private async Task validateLoanRealEstate(List<SalesArrangementSharedParametersLoanRealEstate>? loanRealEstates)
    {
        if (loanRealEstates?.Any(t => t.IsCollateral) ?? false)
        {
            var realEstateTypes = await _codebookService.RealEstateTypes();

            foreach (var estate in loanRealEstates.Where(t => t.IsCollateral))
            {
                if (!(realEstateTypes
                    .FirstOrDefault(t => t.Id == estate.RealEstateTypeId)
                    ?.Collateral ?? false))
                {
                    throw new NobyValidationException(90032);
                }
            }
        }
    }
}