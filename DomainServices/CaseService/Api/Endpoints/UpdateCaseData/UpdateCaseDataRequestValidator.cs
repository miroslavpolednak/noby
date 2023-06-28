using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseData;

internal sealed class UpdateCaseDataRequestValidator
    : AbstractValidator<UpdateCaseDataRequest>
{
    public UpdateCaseDataRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.Data.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProductTypeIdIsEmpty);

        RuleFor(t => (decimal)t.Data.TargetAmount)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TargetAmountIsEmpty);
    }
}
