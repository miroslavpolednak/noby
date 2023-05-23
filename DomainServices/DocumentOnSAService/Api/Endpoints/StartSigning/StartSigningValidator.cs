using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningValidator : AbstractValidator<StartSigningRequest>
{
    public StartSigningValidator()
    {
        RuleFor(e => e.DocumentTypeId).NotNull().WithErrorCode(ErrorCodeMapper.DocumentTypeIdIsRequired);
        RuleFor(e => e.SalesArrangementId).NotNull().WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsRequired);
        RuleFor(e => e.SignatureMethodCode).NotEmpty().WithErrorCode(ErrorCodeMapper.SignatureMethodCodeIsRequired);
    }
}
