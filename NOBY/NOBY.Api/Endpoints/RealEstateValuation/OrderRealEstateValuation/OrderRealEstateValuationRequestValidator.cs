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
            /*RuleFor(t => t.LocalSurveyPerson!.FunctionCode)
                .MustAsync((f, cancel) => (await codebookService.RealEstatePurchaseTypes(cancel)).Any(x => x.Id == f))
                .When(t => !string.IsNullOrEmpty(t.LocalSurveyPerson.FunctionCode));*/
        });
    }
}
