using System.Data;
using CIS.Core.Data;
using CIS.Core.Exceptions;
using CIS.Core.Results;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CodebookService.Abstraction;
using ExternalServices.MpHome.V1._1;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;
using IdentificationDocument = ExternalServices.MpHome.V1._1.MpHomeWrapper.IdentificationDocument;

namespace DomainServices.CustomerService.Api.Services.KonsDb;

public class MpDigiCreateClient
{
    private readonly IMpHomeClient _mpHomeClient;
    private readonly IConnectionProvider _konsDbProvider;
    private readonly ICodebookServiceAbstraction _codebook;

    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;
    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;
    private List<Endpoints.ContactTypes.ContactTypeItem> _contactTypes = null!;

    public MpDigiCreateClient(IMpHomeClient mpHomeClient, IConnectionProvider konsDbProvider, ICodebookServiceAbstraction codebook)
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
        await CheckIfPartnerExists(request.Identity.IdentityId, cancellationToken);

        await InitializeCodebooks(cancellationToken);

        var partnerRequest = MapToPartnerRequest(request);

        var result = await _mpHomeClient.UpdatePartner(request.Identity.IdentityId, partnerRequest);

        if (result is ErrorServiceCallResult error)
            throw new CisServiceCallResultErrorException(error);

        return new Identity(request.Identity.IdentityId, IdentitySchemes.Mp);
    }

    private async Task CheckIfPartnerExists(long partnerId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.PartnerExists, parameters: new { partnerId }, cancellationToken: cancellationToken);

        var exists = await _konsDbProvider.ExecuteDapperQuery(ExistsQuery, cancellationToken);

        if (exists) 
            throw new CisAlreadyExistsException(11017, "Partner already exists in KonsDB."); 

        Task<bool> ExistsQuery(IDbConnection dbConnection) => dbConnection.ExecuteScalarAsync<bool>(command);
    }

    private PartnerRequest MapToPartnerRequest(CreateCustomerRequest request)
    {
        string? citizenship = null;

        if (request.NaturalPerson.CitizenshipCountriesId.Any())
            citizenship = _countries.FirstOrDefault(c => c.Id == request.NaturalPerson.CitizenshipCountriesId.First())?.ShortName;

        var partnerRequest = new PartnerRequest
        {
            Name = request.NaturalPerson.FirstName,
            Lastname = request.NaturalPerson.LastName,
            DegreeBefore = _titles.FirstOrDefault(t => t.Id == request.NaturalPerson.DegreeBeforeId)?.Name,
            BirthNumber = request.NaturalPerson.BirthNumber,
            DateOfBirth = request.NaturalPerson.DateOfBirth,
            PlaceOfBirth = request.NaturalPerson.PlaceOfBirth,
            Gender = (GenderEnum)request.NaturalPerson.GenderId,
            Nationality = citizenship,
            Addresses = request.Addresses.Select(x => new AddressData
            {
                Type = (AddressType)(x.AddressTypeId ?? (int)AddressType.Permanent),
                Street = x.Street,
                BuildingIdentificationNumber = x.BuildingIdentificationNumber,
                LandRegistryNumber = x.LandRegistryNumber,
                PostCode = x.Postcode,
                City = x.City
            }).ToList(),
            Contacts = request.Contacts.Select(c => new ContactRequest
            {
                Type = 0,
                Primary = true,
                Value = c.Value
            }).ToList()
        };

        CreateIdentificationDocument(partnerRequest, request.IdentificationDocument);
        return partnerRequest;
    }

    private void CreateIdentificationDocument(PartnerRequest request, Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return;

        request.IdentificationDocuments.Add(new IdentificationDocument
        {
            Number = document.Number,
            Type = IdentificationCardType.IDCard,
            ValidTo = document.ValidTo,
            IssuedBy = document.IssuedBy,
            IssuedOn = document.IssuedOn,
            IssuingCountry = _countries.FirstOrDefault(c => c.Id == document.IssuingCountryId)?.ShortName
        });
    }
}