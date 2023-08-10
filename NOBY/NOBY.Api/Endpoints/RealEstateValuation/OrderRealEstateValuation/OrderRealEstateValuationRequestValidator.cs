using DomainServices.CodebookService.Clients;
using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

internal sealed class OrderRealEstateValuationRequestValidator
    : AbstractValidator<OrderRealEstateValuationRequest>
{
    public OrderRealEstateValuationRequestValidator(ICodebookServiceClient codebookService)
    {
        When(t => t.LocalSurveyPerson is not null, () =>
        {
            RuleFor(t => t.LocalSurveyPerson!.FunctionCode)
                .MustAsync(async (f, cancel) => (await codebookService.RealEstateValuationLocalSurveyFunctions(cancel)).Any(x => x.Code == f))
                .When(t => !string.IsNullOrEmpty(t.LocalSurveyPerson!.FunctionCode));
        });
    }
}
