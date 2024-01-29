using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Common;

public class DocumentHashValidator : AbstractValidator<DocumentHash>
{
    public DocumentHashValidator()
    {
        RuleFor(request => request.Hash)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.HashRequired)
            .Matches("^([A-Fa-f0-9]{0,1024})$")
                .WithErrorCode(ErrorCodeMapper.HashInvalid);

        RuleFor(request => request.HashAlgorithm)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.HashAlgorithmRequired);
    }
}