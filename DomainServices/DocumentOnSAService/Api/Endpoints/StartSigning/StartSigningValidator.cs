using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningValidator : AbstractValidator<StartSigningRequest>
{
    public StartSigningValidator()
    {
        // ToDo validate DocumentTypeId
        RuleFor(e => e.DocumentTypeId).NotNull().WithMessage($"{nameof(StartSigningRequest.DocumentTypeId)} is required");
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(StartSigningRequest.SalesArrangementId)} is required");
        // ToDo validate SignatureMethodId
        RuleFor(e => e.SignatureMethodId).NotNull().WithMessage($"{nameof(StartSigningRequest.SignatureMethodId)} is required");
    }
}
