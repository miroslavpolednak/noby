using DomainServices.DocumentArchiveService.Api.Endpoints.Common.Validators;
using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentList;

public class GetDocumentListValidator : AbstractValidator<GetDocumentListRequest>
{
    public GetDocumentListValidator()
    {
        RuleFor(t => t).NotEmpty();

        RuleFor(e => e.CreatedOn)
            .Must(CommonValidators.ValidateNotNullDateOnly)
            .WithMessage($"{nameof(GetDocumentListRequest.CreatedOn)}: invalid date format");

        RuleFor(t => t)
        .Must(ValidateOneOfMainParameterIsNotEmpty)
        .WithMessage("One of main parameters have to be fill in (CaseId, PledgeAgreementNumber, ContractNumber, OrderId, AuthorUserLogin)");
    }

    private readonly Func<GetDocumentListRequest, bool> ValidateOneOfMainParameterIsNotEmpty = (request) =>
    {
        if (
              request.CaseId is null &&
              string.IsNullOrWhiteSpace(request.AuthorUserLogin) &&
              string.IsNullOrWhiteSpace(request.PledgeAgreementNumber) &&
              string.IsNullOrWhiteSpace(request.ContractNumber) &&
              request.OrderId is null
            )
        {
            return false;
        }

        return true;
    };

}
