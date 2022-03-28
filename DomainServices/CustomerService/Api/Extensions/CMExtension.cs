using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CustomerService.Api;

internal static class CMExtension
{
    public static string? ToCMstring(this string value)
        => string.IsNullOrEmpty(value) ? null : value;

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
            BuildingIdentificationNumber = componentAddress?.HouseNumber ?? "",
            LandRegistryNumber = componentAddress?.EvidenceNumber ?? "",
            City = model.City ?? "",
            IsPrimary = isPrimary,
            CountryId = countries.FirstOrDefault(t => t.ShortName == model.CountryCode)?.Id,
            Postcode = model.PostCode ?? "",
            Street = (componentAddress?.Street ?? model.Street) ?? ""
        };

    public static Contracts.IdentificationDocument ToIdentificationDocument(this CustomerManagement.CMWrapper.IdentificationDocument model, List<CodebookService.Contracts.Endpoints.Countries.CountriesItem> countries, List<CodebookService.Contracts.Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> identificationDocumentTypes)
        => new ()
        {
            RegisterPlace = model.RegisterPlace ?? "",
            ValidTo = model.ValidTo,
            IssuedOn = model.IssuedOn,
            IssuedBy = model.IssuedBy ?? "",
            Number = model.DocumentNumber ?? "",
            IssuingCountryId = countries.FirstOrDefault(t => t.ShortName == model.IssuingCountryCode)?.Id,
            IdentificationDocumentTypeId = identificationDocumentTypes.First(t => t.RDMCode == model.TypeCode).Id
        };

    public static T CheckCMResult<T>(this IServiceCallResult result)
        => result switch
        {
            SuccessfulServiceCallResult<T> r
            => r.Model,

            SuccessfulServiceCallResult<CustomerManagement.CMWrapper.ApiException<CustomerManagement.CMWrapper.Error>> r
            => r.Model.StatusCode == 404 ?
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, "Customer does not found", 17002) :
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Incorrect inputs to CustomerManagement: {r.Model.Result.Detail}", 17001),

            SuccessfulServiceCallResult <CustomerManagement.CMWrapper.Error> r
            => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"CustomerManagement error: {r.Model.Message}", 17001, new()
            {
                ("cmerrorcode", r.Model.Code),
                ("cmerrortext", r.Model.Message)
            }),

            ErrorServiceCallResult err
                    => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),

            _ => throw new NotImplementedException()
        };
}
