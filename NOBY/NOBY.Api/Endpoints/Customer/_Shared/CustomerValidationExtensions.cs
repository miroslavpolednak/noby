using System.Globalization;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using FluentValidation;
using NOBY.Dto;

namespace NOBY.Api.Endpoints.Customer.Shared;

internal static class CustomerValidationExtensions
{
    public static async Task ValidateMobilePhone(
        this ICustomerServiceClient customerService,
        PhoneNumberDto? mobilePhone,
        CancellationToken cancellationToken)
    {
        if (mobilePhone is null)
            return;

        var phoneNumber = $"{mobilePhone.PhoneIDC}{mobilePhone.PhoneNumber}";

        await ValidateContact(customerService, ContactType.Phone, phoneNumber, cancellationToken);
    }

    public static async Task ValidateEmail(
        this ICustomerServiceClient customerService, 
        EmailAddressDto? emailAddress, 
        CancellationToken cancellationToken)
    {
        if (emailAddress is null)
            return;

        await ValidateContact(customerService, ContactType.Email, emailAddress.EmailAddress, cancellationToken);
    }

    public static IRuleBuilderOptions<T, DateTime> BirthDateValidation<T>(this IRuleBuilder<T, DateTime> ruleBuilder, int errorCode) =>
        ruleBuilder.InclusiveBetween(new DateTime(1850, 1, 1), DateTime.Today).WithErrorCode(errorCode);

    public static IRuleBuilderOptions<T, DateTime?> BirthDateValidation<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, int errorCode) =>
        ruleBuilder.InclusiveBetween(new DateTime(1850, 1, 1), DateTime.Today).WithErrorCode(errorCode);

    public static IRuleBuilderOptions<T, string> BirthNumberValidation<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, DateTime> birthDateGetter, int errorCode)
    {
        return ruleBuilder.NotEmpty().WithErrorCode(errorCode)
                          .Matches(@"^\d+$").WithErrorCode(errorCode)
                          .Length(9, 10).WithErrorCode(errorCode)
                          .Must((request, birthNumber) =>
                          {
                              var checkNumber = int.Parse(birthNumber.AsSpan(6, birthNumber!.Length - 6), CultureInfo.InvariantCulture);

                              if (birthNumber.Length == 9 && checkNumber == 0)
                                  return false;

                              var birthDateByBirthNumber = TryParseBirthNumber(birthNumber);

                              return birthDateByBirthNumber.HasValue && birthDateByBirthNumber.Equals(birthDateGetter(request).Date);
                          }).WithErrorCode(errorCode);
    }

    private static DateTime? TryParseBirthNumber(string birthNumber)
    {
        var year = int.Parse(birthNumber.AsSpan(0, 2), CultureInfo.InvariantCulture);
        var month = int.Parse(birthNumber.AsSpan(2, 2), CultureInfo.InvariantCulture);
        var day = int.Parse(birthNumber.AsSpan(4, 2), CultureInfo.InvariantCulture);

        month -= month > 50 ? 50 : month > 20 ? 20 : 0;

        if (birthNumber.Length == 9)
            year += year > 53 ? 1800 : 1900;
        else
            year += year > 53 ? 1900 : 2000;

        if (month < 1 || month > 12 || day < 1 || day > DateTime.DaysInMonth(year, month))
            return default;

        if (birthNumber.Length == 10 && ulong.Parse(birthNumber, CultureInfo.InvariantCulture) % 11 != 0)
            return default;

        return new DateTime(year, month, day);
    }

    private static async Task ValidateContact(
        ICustomerServiceClient customerService,
        ContactType contactType,
        string contactValue,
        CancellationToken cancellationToken)
    {
        var request = new ValidateContactRequest
        {
            Contact = contactValue,
            ContactType = contactType
        };

        var validationResult = await customerService.ValidateContact(request, cancellationToken);

        if (validationResult.IsContactValid)
            return;

        throw new NobyValidationException(90032, "Contact is not valid");
    }
}