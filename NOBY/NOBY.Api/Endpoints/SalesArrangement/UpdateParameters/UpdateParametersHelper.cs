using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;
using __SA = DomainServices.SalesArrangementService.Contracts;
using System.Text.Json;
using CIS.Core.Attributes;
using _dto = NOBY.Api.Endpoints.SalesArrangement.SharedDto;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

[SelfService, TransientService]
internal sealed class UpdateParametersHelper
{
    public async Task<TModel?> DeserializeAndValidate<TModel>(object? parameters, __SA.SalesArrangement salesArrangement)
        where TModel : class
    {
        if (parameters == null) return null;
        string dataString = ((JsonElement)parameters).GetRawText();
        if (string.IsNullOrEmpty(dataString)) return null;

        var model = JsonSerializer.Deserialize<TModel>(dataString, _jsonSerializerOptions);

        switch (model)
        {
            case _dto.ParametersMortgage m:
                if (string.IsNullOrEmpty(m.IncomeCurrencyCode) || string.IsNullOrEmpty(m.ResidencyCurrencyCode))
                {
                    throw new NobyValidationException(90019);
                }

                await validateLoanRealEstate(m.LoanRealEstates);
                break;

            case Dto.HUBNUpdate m:
                validateExtensionDrawingDate(m.DrawingDateTo.ExtensionDrawingDateToByMonths);
                await validateLoanRealEstate(m.LoanRealEstates);
                await validateApplicant(m.Applicant, salesArrangement.CaseId);
                break;

            case Dto.GeneralChangeUpdate m:
                validateExtensionDrawingDate(m.DrawingDateTo.ExtensionDrawingDateToByMonths);
                await validateApplicant(m.Applicant, salesArrangement.CaseId);
                break;

            case Dto.CustomerChangeUpdate:
                foreach (var applicant in salesArrangement.CustomerChange.Applicants)
                {
                    await validateApplicant(applicant.Identity.Select(identity => (SharedTypes.Types.CustomerIdentity)identity!).ToList(), salesArrangement.CaseId);
                }
                break;

            case _dto.ParametersDrawing m:
                await validateApplicant(m.Applicant, salesArrangement.CaseId);
                
                if (m.PayoutList is null || m.PayoutList.Count == 0)
                {
                    throw new NobyValidationException(90032);
                }
                
                if ((m.Agent?.IsActive ?? false) 
                    && (
                        string.IsNullOrEmpty(m.Agent.FirstName)
                        || string.IsNullOrEmpty(m.Agent.LastName)
                        || string.IsNullOrEmpty(m.Agent.IdentificationDocument?.Number)
                        || m.Agent.IdentificationDocument?.IdentificationDocumentTypeId == null
                        || !m.Agent.DateOfBirth.HasValue
                        )
                    )
                {
                    throw new NobyValidationException(90032);
                }

                if ((m.PayoutList?.Any(t => !_bankAccountValidator.IsBankAccoungAndCodeValid(t)) ?? false)
                    || !_bankAccountValidator.IsBankAccoungAndCodeValid(m.RepaymentAccount))
                {
                    throw new NobyValidationException(90032, "Invalid bank account");
                }

                break;
        }

        return model;
    }

    private void validateExtensionDrawingDate(int? extension)
    {
        if (extension.HasValue && (extension.Value < 1 || extension.Value > 36))
        {
            throw new NobyValidationException("extensionDrawingDateToByMonths must be between 1 and 36");
        }
    }

    private async Task validateApplicant(List<SharedTypes.Types.CustomerIdentity>? identities, long caseId)
    {
        if (!(identities?.Any() ?? false))
        {
            throw new NobyValidationException(90033);
        }

        var customers = await _productService.GetCustomersOnProduct(caseId);
        foreach (var customer in customers.Customers)
        {
            if (customer.CustomerIdentifiers.Any(t => identities.Any(x => (SharedTypes.Enums.IdentitySchemes)t.IdentityScheme == x.Scheme && t.IdentityId == x.Id)))
            {
                return;
            }
        }

        throw new NobyValidationException(90033);
    }

    private async Task validateLoanRealEstate(List<_dto.LoanRealEstateDto>? loanRealEstates)
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

    private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };

    private readonly IProductServiceClient _productService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly Services.Validators.IBankAccountValidatorService _bankAccountValidator;

    public UpdateParametersHelper(ICodebookServiceClient codebookService, IProductServiceClient productService, Services.Validators.IBankAccountValidatorService bankAccountValidator)
    {
        _productService = productService;
        _codebookService = codebookService;
        _bankAccountValidator = bankAccountValidator;
    }
}