using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Common;

public class DocumentHashValidator : AbstractValidator<DocumentHash>
{
    public DocumentHashValidator()
    {
        RuleFor(request => request.Hash)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.HashRequired)
            .MaximumLength(1024)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.HashLengthLimitExceeded);

        RuleFor(request => request.HashAlgorithm)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.HashAlgorithmRequired);
    }
}