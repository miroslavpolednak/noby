using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class DocumentHashValidator 
    : AbstractValidator<Contracts.v2.DocumentHash>
{
    public DocumentHashValidator()
    {
        RuleFor(request => request.Hash)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.HashRequired)
            .Matches("^([A-Fa-f0-9]{0,1024})$")
                .WithErrorCode(ErrorCodeMapper.HashInvalid);

        RuleFor(request => request.HashAlgorithm)
            .Must(x => x != Contracts.v2.DocumentHash.Types.HashAlgorithms.Unknown && Enum.IsDefined(x))
            .WithErrorCode(ErrorCodeMapper.HashAlgorithmRequired);
    }
}