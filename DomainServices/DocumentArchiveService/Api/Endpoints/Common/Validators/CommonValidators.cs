using CIS.Infrastructure.gRPC.CisTypes;
using System.Globalization;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.Common.Validators;

public static class CommonValidators
{
    public static readonly Func<GrpcDate, bool> ValidateDateOnly = (grpcDate) 
        => grpcDate is null ? false : ValidateGrpcDate(grpcDate);
    

    public static readonly Func<GrpcDate, bool> ValidateNotNullDateOnly = (grpcDate)
      => grpcDate is null ? true : ValidateGrpcDate(grpcDate);

    private static bool ValidateGrpcDate(GrpcDate grpcDate)
    {
        var dateInvariant = $"{grpcDate.Month}/{grpcDate.Day}/{grpcDate.Year}";
        return DateOnly.TryParse(dateInvariant, CultureInfo.InvariantCulture, out var dateOnly);
    }
}
