using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.CreateResponseCode;

internal sealed class CreateResponseCodeRequestValidator
    : AbstractValidator<CreateResponseCodeRequest>
{
    public CreateResponseCodeRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.ResponseCodeTypeId)
            .MustAsync(async (id, c) => (await codebookService.ResponseCodeTypes(c)).Any(t => t.Id == id))
            .WithErrorCode(ErrorCodeMapper.ResponseCodeTypeIdNotFound);
    }
}
