using DomainServices.DocumentArchiveService.Api.Endpoints.Common.Validators;
using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentList;

public class GetDocumentListMediatrRequestValidator : AbstractValidator<GetDocumentListMediatrRequest>
{
    public GetDocumentListMediatrRequestValidator()
    {
        RuleFor(t => t.Request).NotEmpty();

        RuleFor(e => e.Request.CreatedOn)
            .Must(CommonValidators.ValidateNotNullDateOnly)
            .WithMessage($"{nameof(GetDocumentListRequest.CreatedOn)}: invalid date format");

        RuleFor(t => t.Request)
        .Must(ValidateOneOfMainParameterIsNotEmpty)
        .WithMessage("One of main parameters have to be fill in (CaseId, PledgeAgreementNumber, ContractNumber, OrderId, FolderDocumentId)");
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
