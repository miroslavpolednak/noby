using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.RefreshElectronicDocExternalId;

public class RefreshElectronicDocExternalIdValidator : AbstractValidator<RefreshElectronicDocExternalIdRequest>
{
    public RefreshElectronicDocExternalIdValidator()
    {
        RuleFor(e => e.ExternalIdESignatures).NotEmpty().WithMessage($"{nameof(RefreshElectronicDocExternalIdRequest.ExternalIdESignaturesFieldNumber)} is required");
    }
}
