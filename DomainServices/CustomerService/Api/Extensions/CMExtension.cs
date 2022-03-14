using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using Grpc.Core;

namespace DomainServices.CustomerService.Api;

internal static class CMExtension
{
    public static string? ToCMstring(this string value)
        => string.IsNullOrEmpty(value) ? null : value;

    public static string ToEmptyString(this string? value)
        => value ?? "";

    public static CIS.Infrastructure.gRPC.CisTypes.Identity ToIdentity(this long customerId)
        => new()
        {
            IdentityId = (int)customerId,
            IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb
        };

    public static Contracts.Address ToAddress(this CustomerManagement.CMWrapper.Address model, CustomerManagement.CMWrapper.ComponentAddress componentAddress, CIS.Foms.Enums.AddressTypes addressType, bool isPrimary, List<CodebookService.Contracts.Endpoints.Countries.CountriesItem> countries)
        => new ()
        {
            AddressTypeId = (int)addressType,
            BuildingIdentificationNumber = (componentAddress?.HouseNumber).ToEmptyString(),
            LandRegistryNumber = (componentAddress?.EvidenceNumber).ToEmptyString(),
            City = model.City.ToEmptyString(),
            IsPrimary = isPrimary,
            CountryId = countries.FirstOrDefault(t => t.ShortName == model.CountryCode)?.Id,
            Postcode = model.PostCode.ToEmptyString(),
            Street = (componentAddress?.Street ?? model.Street).ToEmptyString()
        };

    public static Contracts.IdentificationDocument ToIdentificationDocument(this CustomerManagement.CMWrapper.IdentificationDocument model, List<CodebookService.Contracts.Endpoints.Countries.CountriesItem> countries, List<CodebookService.Contracts.Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> identificationDocumentTypes)
        => new ()
        {
            RegisterPlace = model.RegisterPlace.ToEmptyString(),
            ValidTo = model.ValidTo,
            IssuedOn = model.IssuedOn,
            IssuedBy = model.IssuedBy.ToEmptyString(),
            Number = model.DocumentNumber.ToEmptyString(),
            IssuingCountryId = countries.FirstOrDefault(t => t.ShortName == model.IssuingCountryCode)?.Id,
            IdentificationDocumentTypeId = identificationDocumentTypes.First(t => t.RDMCode == model.TypeCode).Id
        };

    public static T CheckCMResult<T>(this IServiceCallResult result)
        => result switch
        {
            SuccessfulServiceCallResult<T> r
            => r.Model,
            SuccessfulServiceCallResult<CustomerManagement.CMWrapper.ApiException<CustomerManagement.CMWrapper.Error>> r
            => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"Incorrect inputs to CustomerManagement: {r.Model.Result.Message}", 10011),
            SuccessfulServiceCallResult<CustomerManagement.CMWrapper.Error> r
            => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"CustomerManagement error: {r.Model.Message}", 10011, new()
            {
                ("cmerrorcode", r.Model.Code),
                ("cmerrortext", r.Model.Message)
            }),
            SuccessfulServiceCallResult<MpHome.MpHomeWrapper.ApiException> r
                    => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to CustomerManagement", 10011, new()
                    {
                        ("cmerrorcode", r.Model.StatusCode.ToString()),
                        ("cmerrortext", r.Model.Message)
                    }),
            ErrorServiceCallResult err
                    => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };
}
