using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetRealEstateValuationDetail;

internal sealed class GetRealEstateValuationDetailHandler(
    RealEstateValuationServiceDbContext _dbContext,
    IDocumentDataStorage _documentDataStorage,
    Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper)
        : IRequestHandler<GetRealEstateValuationDetailRequest, RealEstateValuationDetail>
{
    public async Task<RealEstateValuationDetail> Handle(GetRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var realEstate = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        var response = new RealEstateValuationDetail
        {
            RealEstateTypeId = realEstate.RealEstateTypeId,
            CaseId = realEstate.CaseId,
            IsLoanRealEstate = realEstate.IsLoanRealEstate,
            DeveloperApplied = realEstate.DeveloperApplied,
            DeveloperAllowed = realEstate.DeveloperAllowed,
            RealEstateValuationId = realEstate.RealEstateValuationId,
            ValuationStateId = realEstate.ValuationStateId,
            ValuationTypeId = (ValuationTypes)realEstate.ValuationTypeId,
            IsRevaluationRequired = realEstate.IsRevaluationRequired,
            ValuationSentDate = realEstate.ValuationSentDate,
            RealEstateStateId = realEstate.RealEstateStateId,
            Address = realEstate.Address,
            OrderId = realEstate.OrderId,
            PreorderId = realEstate.PreorderId,
            IsOnlineDisqualified = realEstate.IsOnlineDisqualified,
            RealEstateSubtypeId = realEstate.RealEstateSubtypeId,
            ACVRealEstateTypeId = realEstate.ACVRealEstateTypeId,
            BagmanRealEstateTypeId = realEstate.BagmanRealEstateTypeId,
            Comment = realEstate.Comment
        };

        if (realEstate.Prices is not null)
        {
            response.Prices.AddRange(realEstate.Prices.Select(t => new PriceDetail
            {
                Price = t.Price,
                PriceSourceType = t.PriceSourceType
            }));
        }

        if (realEstate.PossibleValuationTypeId is not null)
        {
            response.PossibleValuationTypeId.AddRange(realEstate.PossibleValuationTypeId);
        }

        // attachments
        response.Attachments.AddRange(await getAttachments(request.RealEstateValuationId, cancellationToken));

        var revDetail = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.RealEstateValudationData, int>(request.RealEstateValuationId, cancellationToken);
        _mapper.MapFromDataToSingle(revDetail?.Data, response);

        return response;
    }

    private async Task<List<RealEstateValuationAttachment>> getAttachments(int realEstateValuationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Attachments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == realEstateValuationId)
            .Select(t => new RealEstateValuationAttachment
            {
                RealEstateValuationAttachmentId = t.RealEstateValuationAttachmentId,
                Title = t.Title,
                FileName = t.FileName,
                ExternalId = t.ExternalId,
                AcvAttachmentCategoryId = t.AcvAttachmentCategoryId,
                CreatedOn = t.CreatedTime
            })
            .ToListAsync(cancellationToken);
    }
}
