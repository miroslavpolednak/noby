
using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CustomerService.Api;

internal static class MpHomeExtensions
{
    public static MpHome.MpHomeWrapper.PartnerRequest ToMpHomePartner(this Contracts.CreateRequest model)
    {
        // vytvorit partnerRequest
        var partner = new MpHome.MpHomeWrapper.PartnerRequest()
        {
            BirthNumber = model.BirthNumber,
            DateOfBirth = model.DateOfBirth,
            DegreeAfter = model.DegreeAfter,
            DegreeBefore = model.DegreeBefore,
            Gender = model.Gender switch
            {
                Contracts.Genders.Male => MpHome.MpHomeWrapper.GenderEnum.Male,
                Contracts.Genders.Female => MpHome.MpHomeWrapper.GenderEnum.Female,
                _ => MpHome.MpHomeWrapper.GenderEnum.Unknown
            },
            Ico = model.Ico,
            Lastname = model.LastName,
            Name = model.FirstName,
            NameJuridicalPerson = model.NameJuridicalPerson,
            PlaceOfBirth = model.PlaceOfBirth,
            IdentificationDocuments = new List<MpHome.MpHomeWrapper.IdentificationDocument>(),
            Addresses = new List<MpHome.MpHomeWrapper.AddressData>(),
            Contacts = new List<MpHome.MpHomeWrapper.ContactRequest>()
        };

        // pridat doklad
        if (model.IdentificationDocument != null)
            partner.IdentificationDocuments.Add(model.IdentificationDocument.ToMpHomeIdentificationDocument());

        // pridat adresy
        if (model.Addresses != null && model.Addresses.Any())
            model.Addresses.ToList().ForEach(address => partner.Addresses.Add(address.ToMpHomeAddress()));

        // pridat kontakty
        if (model.Contacts != null && model.Contacts.Any())
            model.Contacts.ToList().ForEach(contact => partner.Contacts.Add(new MpHome.MpHomeWrapper.ContactRequest
            {
                Primary = contact.IsPrimary,
                Value = contact.Value,
                RequestedAction = MpHome.MpHomeWrapper.ActionType.Create,
                Type = contact.Type switch
                {
                    Contracts.Contact.Types.ContactTypes.Email => MpHome.MpHomeWrapper.ContactType.Email,
                    Contracts.Contact.Types.ContactTypes.MobilePrivate => MpHome.MpHomeWrapper.ContactType.Mobile,
                    Contracts.Contact.Types.ContactTypes.MobileWork => MpHome.MpHomeWrapper.ContactType.BusinessMobile,
                    Contracts.Contact.Types.ContactTypes.LandlineHome => MpHome.MpHomeWrapper.ContactType.FixedHomeLine,
                    _ => MpHome.MpHomeWrapper.ContactType.Unknown
                }
            }));

        return partner;
    }

    public static MpHome.MpHomeWrapper.PartnerBaseRequest ToMpHomePartnerBase(this Contracts.CustomerInput model)
        => new ()
        {
            BirthNumber = model.BirthNumber,
            DateOfBirth = model.DateOfBirth,
            DegreeAfter = model.DegreeAfter,
            DegreeBefore = model.DegreeBefore,
            Gender = model.Gender switch
            {
                Contracts.Genders.Male => MpHome.MpHomeWrapper.GenderEnum.Male,
                Contracts.Genders.Female => MpHome.MpHomeWrapper.GenderEnum.Female,
                _ => MpHome.MpHomeWrapper.GenderEnum.Unknown
            },
            Ico = model.Ico,
            Lastname = model.LastName,
            Name = model.FirstName,
            NameJuridicalPerson = model.NameJuridicalPerson,
            PlaceOfBirth = model.PlaceOfBirth
        };

    public static MpHome.MpHomeWrapper.IdentificationDocument ToMpHomeIdentificationDocument(this Contracts.IdentificationDocument model)
        => new ()
        {
            Type = model.Type switch
            {
                    //TODO IdentificationDocumentTypes na MpHome IdentificationCardType
                    Contracts.IdentificationDocumentTypes.A => MpHome.MpHomeWrapper.IdentificationCardType.IDCard,
                Contracts.IdentificationDocumentTypes.B => MpHome.MpHomeWrapper.IdentificationCardType.Passport,
                _ => MpHome.MpHomeWrapper.IdentificationCardType.Undefined
            },
            IssuedBy = model.IssuedBy,
            IssuedOn = model.IssuedOn,
            IssuingCountry = model.IssuingCountryCode,
            ValidTo = model.ValidTo,
            Number = model.Number
        };

    public static MpHome.MpHomeWrapper.AddressData ToMpHomeAddress(this Contracts.Address model)
        => new ()
        {
            BuildingIdentificationNumber = model.BuildingIdentificationNumber,
            City = model.City,
            LandRegistryNumber = model.LandRegistryNumber,
            PostCode = model.Postcode,
            Street = model.Street,
            Type = model.Type switch
            {
                Contracts.AddressTypes.Pernament => MpHome.MpHomeWrapper.AddressType.Permanent,
                Contracts.AddressTypes.Mailing => MpHome.MpHomeWrapper.AddressType.Mailing,
                _ => MpHome.MpHomeWrapper.AddressType.Unknown,
            }
        };

    public static MpHome.MpHomeWrapper.ContactData ToMpHomeContactData(this Contracts.Contact model)
        => new ()
        {
            Primary = model.IsPrimary,
            Value = model.Value,
            Type = model.Type switch
            {
                Contracts.Contact.Types.ContactTypes.Email => MpHome.MpHomeWrapper.ContactType.Email,
                Contracts.Contact.Types.ContactTypes.MobilePrivate => MpHome.MpHomeWrapper.ContactType.Mobile,
                Contracts.Contact.Types.ContactTypes.MobileWork => MpHome.MpHomeWrapper.ContactType.BusinessMobile,
                Contracts.Contact.Types.ContactTypes.LandlineHome => MpHome.MpHomeWrapper.ContactType.FixedHomeLine,
                _ => MpHome.MpHomeWrapper.ContactType.Unknown
            }
        };

    // TODO: jinak, jinam + status, code, text ...
    public static T ToMpHomeResult<T>(this IServiceCallResult result) {
        CheckMpHomeResult(result);

        var ret = result switch
        {
            SuccessfulServiceCallResult<T> r => r.Model,
            _ => throw new NotImplementedException()
        };

        return ret;
    }

    public static void CheckMpHomeResult(this IServiceCallResult result) {

        switch (result) {
            case SuccessfulServiceCallResult<MpHome.MpHomeWrapper.ApiException<MpHome.MpHomeWrapper.ModelErrorWrapper>> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"Incorrect inputs to MpHome: {r.Model.Result.Title}", 10011, r.Model.Result.Errors.Select(s => (s.Key, Value: s.Value.First())).ToList());
            case SuccessfulServiceCallResult<MpHome.MpHomeWrapper.Error> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"MpHome error: {r.Model.Title}", 10011, new()
                {
                    ("mphomeerrorcode", r.Model.Code),
                    ("mphomeerrortext", r.Model.Message)
                });
            case SuccessfulServiceCallResult<MpHome.MpHomeWrapper.ProblemDetails> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"MpHome error: {r.Model.Title}", 10011, new()
                {
                    ("mphomeerrorcode", r.Model.Status?.ToString() ?? "404"),
                    ("mphomeerrortext", "Not Found")
                });
            case SuccessfulServiceCallResult<MpHome.MpHomeWrapper.ApiException> r:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to MpHome", 10011, new()
                {
                    ("mphomeerrorcode", r.Model.StatusCode.ToString()),
                    ("mphomeerrortext", r.Model.Message)
                });
            case ErrorServiceCallResult err:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key);
        }

    }
}