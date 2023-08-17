﻿using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.Dto;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

public interface IPreorderServiceClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<bool> RevaluationCheck(Contracts.OnlineRevaluationCheckRequestDTO request, CancellationToken cancellationToken = default);

    Task<List<CIS.Foms.Enums.RealEstateValuationTypes>> GetValuationTypes(Contracts.AvailableValuationTypesRequestDTO request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Upload souboru s prilohou
    /// </summary>
    /// <param name="title">Popis souboru</param>
    /// <param name="fileName">Název souboru</param>
    /// <param name="mimeType">MIME type</param>
    /// <param name="fileData">Obsah souboru</param>
    Task<long> UploadAttachment(string title, string category, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken = default);

    Task DeleteAttachment(long externalId, CancellationToken cancellationToken = default);

    Task<OrderResponse> CreateOrder(object request, CancellationToken cancellationToken = default);
}
