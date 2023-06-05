using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementParameters;

internal sealed class UpdateSalesArrangementParametersRequestValidator
    : AbstractValidator<Contracts.UpdateSalesArrangementParametersRequest>
{
    public UpdateSalesArrangementParametersRequestValidator(CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.Mortgage)
            .SetValidator(new SalesArrangementParametersMortgageValidator(codebookService))
            .When(t => t.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage);

        RuleForEach(t => t.Mortgage.LoanRealEstates)
            .SetValidator(new MortgageLoanRealEstateValidator(codebookService))
            .When(t => t.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage); ;
    }
}

internal sealed class SalesArrangementParametersMortgageValidator
    : AbstractValidator<Contracts.SalesArrangementParametersMortgage>
{
    public SalesArrangementParametersMortgageValidator(CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.IncomeCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return string.IsNullOrEmpty(code) ||  
                    (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithErrorCode(ErrorCodeMapper.IncomeCurrencyCodeNotFound);
        
        RuleFor(t => t.ResidencyCurrencyCode)
            .MustAsync(async (code, cancellation) =>
            {
                return string.IsNullOrEmpty(code) || 
                    (await codebookService.Currencies(cancellation)).Any(t => t.Code == code);
            })
            .WithErrorCode(ErrorCodeMapper.ResidencyCurrencyCodeNotFound);

        RuleFor(t => t.ContractSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithErrorCode(ErrorCodeMapper.ContractSignatureTypeNotFound);

        RuleForEach(t => t.LoanRealEstates)
            .SetValidator(new MortgageLoanRealEstateValidator(codebookService));
    }
};

internal sealed class MortgageLoanRealEstateValidator
    : AbstractValidator<Contracts.SalesArrangementParametersMortgage.Types.LoanRealEstate>
{
    public MortgageLoanRealEstateValidator(CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.RealEstateTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return (await codebookService.RealEstateTypes(cancellation)).Any(t => t.Id == id);
            })
            .WithErrorCode(ErrorCodeMapper.RealEstateTypeIdNotFound);
    }
};