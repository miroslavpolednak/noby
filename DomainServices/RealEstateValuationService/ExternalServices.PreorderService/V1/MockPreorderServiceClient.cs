﻿using SharedTypes.Enums;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.Dto;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1.Contracts;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

internal sealed class MockPreorderServiceClient
    : IPreorderServiceClient
{
    public Task<OrderResponse> CreateOrder(object request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAttachment(long externalId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<List<(int Price, string PriceSourceType)>?> GetOrderResult(long orderId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<EnumRealEstateValuationTypes>> GetValuationTypes(AvailableValuationTypesRequestDTO request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RevaluationCheck(OnlineRevaluationCheckRequestDTO request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> UploadAttachment(string title, string category, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<long>((new Random()).Next(1, 1000));
    }
}
