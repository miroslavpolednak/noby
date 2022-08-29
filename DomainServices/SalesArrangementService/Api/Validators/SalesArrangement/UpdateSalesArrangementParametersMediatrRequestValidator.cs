using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateSalesArrangementParametersMediatrRequestValidator
    : AbstractValidator<Dto.UpdateSalesArrangementParametersMediatrRequest>
{
    public UpdateSalesArrangementParametersMediatrRequestValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("16010");

        RuleFor(t => t.Request.Mortgage)
            .SetValidator(new SalesArrangementParametersMortgageValidator(codebookService))
            .When(t => t.Request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage);
       
        RuleForEach(t => t.Request.Mortgage.LoanRealEstates)
            .SetValidator(new MortgageLoanRealEstateValidator(codebookService))
            .When(t => t.Request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage); ;
    }
}

internal class SalesArrangementParametersMortgageValidator
    : AbstractValidator<Contracts.SalesArrangementParametersMortgage>
{
    public SalesArrangementParametersMortgageValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.IncomeCurrencyCode)
            .NotEmpty()
            .WithMessage("IncomeCurrencyCode is empty").WithErrorCode("16034");
        RuleFor(t => t.IncomeCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithMessage("IncomeCurrencyCode not found").WithErrorCode("16059");

        RuleFor(t => t.ResidencyCurrencyCode)
            .NotEmpty()
            .WithMessage("ResidencyCurrencyCode is empty").WithErrorCode("16035");
        RuleFor(t => t.ResidencyCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithMessage("ResidencyCurrencyCode not found").WithErrorCode("16060");

        RuleFor(t => t.ContractSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithMessage("ContractSignatureTypeId not found").WithErrorCode("99999"); // TODO: Error code (16061)

        RuleFor(t => t.SalesArrangementSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithMessage("SalesArrangementSignatureTypeId not found").WithErrorCode("99999"); // TODO: Error code

        RuleForEach(t => t.LoanRealEstates)
            .SetValidator(new MortgageLoanRealEstateValidator(codebookService));
    }
};

internal class MortgageLoanRealEstateValidator
    : AbstractValidator<Contracts.SalesArrangementParametersMortgage.Types.LoanRealEstate>
{
    public MortgageLoanRealEstateValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.RealEstateTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return (await codebookService.RealEstateTypes(cancellation)).Any(t => t.Id == id);
            })
            .WithMessage("RealEstateTypeId not found").WithErrorCode("16037");
    }
};