using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;
using __SA = DomainServices.SalesArrangementService.Contracts;
using System.Text.Json;
using _dto = NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

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
                await validateLoanRealEstate(m.LoanRealEstates);
                await validateApplicant(m.Applicant, salesArrangement.CaseId);
                break;

            case Dto.GeneralChangeUpdate m:
                await validateApplicant(m.Applicant, salesArrangement.CaseId);
                break;

            case Dto.CustomerChangeUpdate:
                foreach (var applicant in salesArrangement.CustomerChange.Applicants)
                {
                    await validateApplicant(applicant.Identity?.Cast<CIS.Foms.Types.CustomerIdentity>()?.ToList(), salesArrangement.CaseId);
                }
                break;

            case _dto.ParametersDrawing m:
                await validateApplicant(m.Applicant, salesArrangement.CaseId);
                break;
        }

        return model;
    }

    private async Task validateApplicant(List<CIS.Foms.Types.CustomerIdentity>? identities, long caseId)
    {
        if (!(identities?.Any() ?? false))
        {
            throw new NobyValidationException(90033);
        }

        var customers = await _productService.GetCustomersOnProduct(caseId);
        foreach (var customer in customers.Customers)
        {
            if (customer.CustomerIdentifiers.Any(t => identities.Any(x => (CIS.Foms.Enums.IdentitySchemes)t.IdentityScheme == x.Scheme && t.IdentityId == x.Id)))
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

    public UpdateParametersHelper(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}