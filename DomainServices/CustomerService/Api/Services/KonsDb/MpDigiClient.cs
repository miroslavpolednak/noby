using System.Data;
using CIS.Core.Data;
using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using ExternalServices.MpHome.V1_1;
using ExternalServices.MpHome.V1_1.Contracts;
using FastEnumUtility;
using Google.Protobuf.Collections;
using IdentificationDocument = ExternalServices.MpHome.V1_1.Contracts.IdentificationDocument;

namespace DomainServices.CustomerService.Api.Services.KonsDb;

[ScopedService, SelfService]
public class MpDigiClient
{
    private readonly IMpHomeClient _mpHomeClient;
    private readonly IConnectionProvider _konsDbProvider;
    private readonly ICodebookServiceClient _codebook;

    private List<GenericCodebookResponse.Types.GenericCodebookItem> _titles = null!;
    private List<CountriesResponse.Types.CountryItem> _countries = null!;
    private List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> _docTypes = null!;
    private List<ContactTypesResponse.Types.ContactTypeItem> _contactTypes = null!;

    public MpDigiClient(IMpHomeClient mpHomeClient, IConnectionProvider konsDbProvider, ICodebookServiceClient codebook)
    {
        _mpHomeClient = mpHomeClient;
        _konsDbProvider = konsDbProvider;
        _codebook = codebook;
    }

    private Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        return Task.WhenAll(Titles(), Countries(), DocTypes(), ContactTypes());

        async Task Titles() => _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
        async Task Countries() => _countries = await _codebook.Countries(cancellationToken);
        async Task DocTypes() => _docTypes = await _codebook.IdentificationDocumentTypes(cancellationToken);
        async Task ContactTypes() => _contactTypes = await _codebook.ContactTypes(cancellationToken);
    }

    public async Task<Identity> CreatePartner(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var mpIdentity = request.Identities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);

        var exists = await PartnerExists(mpIdentity.IdentityId, cancellationToken);

        if (exists)
        {
            throw new CisAlreadyExistsException(11017, "Partner already exists in KonsDB."); 
        }

        await InitializeCodebooks(cancellationToken);

        var partnerRequest = MapToPartnerRequest(request);

        await _mpHomeClient.UpdatePartner(mpIdentity.IdentityId, partnerRequest, cancellationToken);

        return new Identity(mpIdentity.IdentityId, IdentitySchemes.Mp);
    }

    public async Task UpdatePartner(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var mpIdentity = request.Identities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);
        
        var exists = await PartnerExists(mpIdentity.IdentityId, cancellationToken);

        if (!exists)
        {
            // todo: error code
            throw new CisNotFoundException(9999999, "Partner does not exist in KonsDB."); 
        }
        
        await InitializeCodebooks(cancellationToken);

        var partnerRequest = MapToPartnerRequest(request);
        
        await _mpHomeClient.UpdatePartner(mpIdentity.IdentityId, partnerRequest, cancellationToken);
    }

    private async Task<bool> PartnerExists(long partnerId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.PartnerExists, parameters: new { partnerId }, cancellationToken: cancellationToken);

        return await _konsDbProvider.ExecuteDapperQueryAsync(ExistsQuery, cancellationToken);
        
        Task<bool> ExistsQuery(IDbConnection dbConnection) => dbConnection.ExecuteScalarAsync<bool>(command);
    }

    private AddressData MapToAddressData(GrpcAddress address)
    {
        return new AddressData
        {
            Type = (AddressType)(address.AddressTypeId ?? (int)AddressType.Permanent),
            Street = address.Street,
            BuildingIdentificationNumber = address.StreetNumber,
            LandRegistryNumber = string.IsNullOrWhiteSpace(address.EvidenceNumber) ? address.HouseNumber : address.EvidenceNumber,
            PostCode = address.Postcode,
            City = address.City
        };
    }

    private ContactRequest MapToContactRequest(Contact contact)
    {
        return contact.ToExternalService(_contactTypes);
    }
    
    private PartnerRequest MapToPartnerRequest(CreateCustomerRequest request)
    {
        var partnerRequest = new PartnerRequest
        {
            Name = request.NaturalPerson.FirstName,
            Lastname = request.NaturalPerson.LastName,
            DegreeBefore = ExtractDegreeBefore(request.NaturalPerson),
            BirthNumber = request.NaturalPerson.BirthNumber,
            DateOfBirth = request.NaturalPerson.DateOfBirth,
            PlaceOfBirth = request.NaturalPerson.PlaceOfBirth,
            Gender = (GenderEnum)request.NaturalPerson.GenderId,
            Nationality = ExtractCitizenship(request.NaturalPerson),
            Addresses = request.Addresses.Select(MapToAddressData).ToList(),
            Contacts = request.Contacts.Select(MapToContactRequest).ToList(),
            KbId = ExtractKbId(request.Identities)
        };

        CreateIdentificationDocument(partnerRequest, request.IdentificationDocument);
        return partnerRequest;
    }

    private PartnerRequest MapToPartnerRequest(UpdateCustomerRequest request)
    {
        var partnerRequest = new PartnerRequest
        {
            Name = request.NaturalPerson.FirstName,
            Lastname = request.NaturalPerson.LastName,
            DegreeBefore = ExtractDegreeBefore(request.NaturalPerson),
            BirthNumber = request.NaturalPerson.BirthNumber,
            DateOfBirth = request.NaturalPerson.DateOfBirth,
            PlaceOfBirth = request.NaturalPerson.PlaceOfBirth,
            Gender = (GenderEnum)request.NaturalPerson.GenderId,
            Nationality = ExtractCitizenship(request.NaturalPerson),
            Addresses = request.Addresses.Select(MapToAddressData).ToList(),
            Contacts = request.Contacts.Select(MapToContactRequest).ToList(),
            KbId = ExtractKbId(request.Identities)
        };

        CreateIdentificationDocument(partnerRequest, request.IdentificationDocument);
        return partnerRequest;
    }

    private string? ExtractCitizenship(NaturalPerson naturalPerson)
    {
        return _countries.FirstOrDefault(c => c.Id == naturalPerson.CitizenshipCountriesId.FirstOrDefault())?.ShortName;
    }
    
    private string? ExtractDegreeBefore(NaturalPerson naturalPerson)
    {
        return _titles.FirstOrDefault(t => t.Id == naturalPerson.DegreeBeforeId)?.Name;
    }

    private static long? ExtractKbId(RepeatedField<Identity> identities)
    {
        return identities
            .Where(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
            .Select(i => (long?)i.IdentityId).FirstOrDefault();
    }
    
    private void CreateIdentificationDocument(PartnerRequest request, Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return;

        request.IdentificationDocuments = new List<IdentificationDocument>()
        {
            new IdentificationDocument
            {
                Number = document.Number,
                Type = FastEnum.Parse<IdentificationCardType>(_docTypes.First(d => d.Id == document.IdentificationDocumentTypeId).MpDigiApiCode),
                ValidTo = document.ValidTo,
                IssuedBy = document.IssuedBy,
                IssuedOn = document.IssuedOn,
                IssuingCountry = _countries.FirstOrDefault(c => c.Id == document.IssuingCountryId)?.ShortName ?? ""
            }
        };
    }
}