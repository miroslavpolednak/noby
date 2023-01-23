using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningValidator : AbstractValidator<StartSigningRequest>
{
    public StartSigningValidator()
    {
        RuleFor(e => e.DocumentTypeId).NotNull().WithMessage($"{nameof(StartSigningRequest.DocumentTypeId)} is required");
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(StartSigningRequest.SalesArrangementId)} is required");
        RuleFor(e => e.SignatureMethodCode).NotEmpty().WithMessage($"{nameof(StartSigningRequest.SignatureMethodCode)} is required");
    }
}
