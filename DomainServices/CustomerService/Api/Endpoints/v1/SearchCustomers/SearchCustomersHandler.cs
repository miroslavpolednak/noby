﻿using System.ComponentModel;
using DomainServices.CustomerService.Api.Services;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using ExternalServices.MpHome.V1;

namespace DomainServices.CustomerService.Api.Endpoints.v1.SearchCustomers;

internal sealed class SearchCustomersHandler(
    CustomerManagementSearchProvider _cmSearchProvider,
    IMpHomeClient _mpHome,
    MpHomeDetailMapper _mpHomeDetailMapper)
    : IRequestHandler<SearchCustomersRequest, SearchCustomersResponse>
{
    public async Task<SearchCustomersResponse> Handle(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        var response = new SearchCustomersResponse();

        response.Customers.AddRange(request.Mandant switch
        {
            SharedTypes.GrpcTypes.Mandants.Kb => await _cmSearchProvider.Search(request, cancellationToken),
            SharedTypes.GrpcTypes.Mandants.Mp => await searchMpHome(request, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        });

        return response;
    }

    private async Task<List<SearchCustomersItem>> searchMpHome(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        var searchRequest = new global::ExternalServices.MpHome.V1.Contracts.PartnerSearchRequest
        {
            BirthNumber = request.NaturalPerson?.BirthNumber,
            DateOfBirth = request.NaturalPerson?.DateOfBirth,
            LastName = request.NaturalPerson?.LastName,
            Name = request.NaturalPerson?.FirstName,
            Mobile = $"{request.MobilePhone?.PhoneIDC}{request.MobilePhone?.PhoneNumber}",
            Email = request.Email?.EmailAddress,
            IdentificationDocument = request.IdentificationDocument is null ? null : new()
            {
                Number = request.IdentificationDocument.Number,
                IssuingCountryId = request.IdentificationDocument.IssuingCountryId,
                Type = (global::ExternalServices.MpHome.V1.Contracts.IdentificationCardType)request.IdentificationDocument.IdentificationDocumentTypeId
            }
        };
#pragma warning restore CS8601 // Possible null reference assignment.
        var response = await _mpHome.SearchPartners(searchRequest, cancellationToken);

        return await _mpHomeDetailMapper.MapSearchResponse(response, cancellationToken);
    }
}