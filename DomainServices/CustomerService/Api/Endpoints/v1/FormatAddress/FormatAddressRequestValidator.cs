using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.FormatAddress;

internal sealed class FormatAddressRequestValidator
    : AbstractValidator<FormatAddressRequest>
{
    public FormatAddressRequestValidator()
    {
        RuleFor(r => r.Address.City)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.AddressDataMissing);

        RuleFor(r => r.Address.CountryId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.AddressDataMissing);
    }
}