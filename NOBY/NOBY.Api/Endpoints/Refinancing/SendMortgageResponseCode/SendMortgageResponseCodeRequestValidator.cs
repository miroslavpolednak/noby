using DomainServices.CodebookService.Clients;
using FluentValidation;

namespace NOBY.Api.Endpoints.Refinancing.SendMortgageResponseCode;

internal sealed class SendMortgageResponseCodeRequestValidator
    : AbstractValidator<SendMortgageResponseCodeRequest>
{
    public SendMortgageResponseCodeRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.ResponseCodeTypeId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (id, c) => (await codebookService.ResponseCodes(c)).Any(t => t.Id == id))
            .WithMessage("Unknown ResponseCodeTypeId")
            .MustAsync(async (req, id, c) =>
            {
                var cb = (await codebookService.ResponseCodes(c)).First(t => t.Id == id);
                return cb.DataType switch
                {
                    DomainServices.CodebookService.Contracts.v1.ResponseCodesResponse.Types.ResponseCodesItemDataTypes.Date => req.DataDateTime.HasValue,
                    DomainServices.CodebookService.Contracts.v1.ResponseCodesResponse.Types.ResponseCodesItemDataTypes.BankCode => (await codebookService.BankCodes(c)).Any(t => t.BankCode == req.DataBankCode),
                    _ => true
                };
            })
            .WithMessage("Unknown ResponseCodeTypeId");
    }
}
