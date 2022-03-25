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
            .WithMessage("IncomeCurrencyCode is empty").WithErrorCode("16xxx");
        RuleFor(t => t.IncomeCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithMessage("IncomeCurrencyCode not found").WithErrorCode("16xxx");

        RuleFor(t => t.ResidencyCurrencyCode)
            .NotEmpty()
            .WithMessage("ResidencyCurrencyCode is empty").WithErrorCode("16xxx");
        RuleFor(t => t.ResidencyCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithMessage("ResidencyCurrencyCode not found").WithErrorCode("16xxx");

        RuleFor(t => t.SignatureTypeId)
           .GreaterThan(0)
           .WithMessage("SignatureTypeId is empty").WithErrorCode("16xxx");
        RuleFor(t => t.SignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id);
            })
            .WithMessage("SignatureTypeId not found").WithErrorCode("16xxx");

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
           .GreaterThan(0)
           .WithMessage("RealEstateTypeId is empty").WithErrorCode("16xxx");
        RuleFor(t => t.RealEstateTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id);
            })
            .WithMessage("RealEstateTypeId not found").WithErrorCode("16xxx");
    }
};