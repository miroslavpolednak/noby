using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationList;

internal sealed class GetRealEstateValuationListRequestValidator
    : AbstractValidator<GetRealEstateValuationListRequest>
{
    public GetRealEstateValuationListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdEmpty);
    }
}
