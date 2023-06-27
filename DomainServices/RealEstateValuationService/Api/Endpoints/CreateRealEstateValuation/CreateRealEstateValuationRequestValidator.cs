using DomainServices.CodebookService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationRequestValidator
    : AbstractValidator<CreateRealEstateValuationRequest>
{
    public CreateRealEstateValuationRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdEmpty);

        RuleFor(t => t.RealEstateTypeId)
            .MustAsync(async (t, cancellationToken) => (await codebookService.RealEstateTypes(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.RealEstateTypeIdNotFound);

        RuleFor(t => t.ValuationStateId)
            .MustAsync(async (t, cancellationToken) => (await codebookService.WorkflowTaskStatesNoby(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.ValuationStateIdNotFound);
    }
}
