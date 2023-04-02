using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementParameters;

internal class UpdateSalesArrangementParametersRequestValidator
    : AbstractValidator<Contracts.UpdateSalesArrangementParametersRequest>
{
    public UpdateSalesArrangementParametersRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");

        RuleFor(t => t.Mortgage)
            .SetValidator(new SalesArrangementParametersMortgageValidator(codebookService))
            .When(t => t.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage);

        RuleForEach(t => t.Mortgage.LoanRealEstates)
            .SetValidator(new MortgageLoanRealEstateValidator(codebookService))
            .When(t => t.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage); ;


    }
}

internal class SalesArrangementParametersMortgageValidator
    : AbstractValidator<Contracts.SalesArrangementParametersMortgage>
{
    public SalesArrangementParametersMortgageValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.IncomeCurrencyCode)
            .NotEmpty()
            .WithMessage("IncomeCurrencyCode is empty").WithErrorCode("18034");
        RuleFor(t => t.IncomeCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithMessage("IncomeCurrencyCode not found").WithErrorCode("18059");

        RuleFor(t => t.ResidencyCurrencyCode)
            .NotEmpty()
            .WithMessage("ResidencyCurrencyCode is empty").WithErrorCode("18035");
        RuleFor(t => t.ResidencyCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithMessage("ResidencyCurrencyCode not found").WithErrorCode("18060");

        RuleFor(t => t.ContractSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithMessage("ContractSignatureTypeId not found").WithErrorCode("18061");

        RuleForEach(t => t.LoanRealEstates)
            .SetValidator(new MortgageLoanRealEstateValidator(codebookService));
    }
};

internal sealed class MortgageLoanRealEstateValidator
    : AbstractValidator<Contracts.SalesArrangementParametersMortgage.Types.LoanRealEstate>
{
    public MortgageLoanRealEstateValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.RealEstateTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return (await codebookService.RealEstateTypes(cancellation)).Any(t => t.Id == id);
            })
            .WithMessage("RealEstateTypeId not found").WithErrorCode("18037");
    }
};