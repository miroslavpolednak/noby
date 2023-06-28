using DomainServices.CodebookService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.PatchDeveloperOnRealEstateValuation;

internal sealed class PatchDeveloperOnRealEstateValuationRequestValidator
    : AbstractValidator<PatchDeveloperOnRealEstateValuationRequest>
{
    public PatchDeveloperOnRealEstateValuationRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);

        RuleFor(t => t.ValuationStateId)
            .MustAsync(async (t, cancellationToken) => (await codebookService.WorkflowTaskStatesNoby(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.ValuationStateIdNotFound);
    }
}
