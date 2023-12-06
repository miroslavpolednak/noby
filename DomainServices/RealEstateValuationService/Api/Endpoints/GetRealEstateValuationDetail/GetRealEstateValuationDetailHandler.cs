using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationDetail;

internal sealed class GetRealEstateValuationDetailHandler
    : IRequestHandler<GetRealEstateValuationDetailRequest, RealEstateValuationDetail>
{
    public async Task<RealEstateValuationDetail> Handle(GetRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var realEstate = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        var response = new RealEstateValuationDetail
        {
            RealEstateTypeId = realEstate.RealEstateTypeId,
            CaseId = realEstate.CaseId,
            IsLoanRealEstate = realEstate.IsLoanRealEstate,
            DeveloperApplied = realEstate.DeveloperApplied,
            DeveloperAllowed = realEstate.DeveloperAllowed,
            RealEstateValuationId = realEstate.RealEstateValuationId,
            ValuationStateId = realEstate.ValuationStateId,
            ValuationTypeId = (Contracts.ValuationTypes)realEstate.ValuationTypeId,
            IsRevaluationRequired = realEstate.IsRevaluationRequired,
            ValuationSentDate = realEstate.ValuationSentDate,
            RealEstateStateId = realEstate.RealEstateStateId,
            Address = realEstate.Address,
            OrderId = realEstate.OrderId,
            PreorderId = realEstate.PreorderId,
            IsOnlineDisqualified = realEstate.IsOnlineDisqualified,
            ValuationResultCurrentPrice = realEstate.ValuationResultCurrentPrice,
            ValuationResultFuturePrice = realEstate.ValuationResultFuturePrice,
            RealEstateSubtypeId = realEstate.RealEstateSubtypeId,
            ACVRealEstateTypeId = realEstate.ACVRealEstateTypeId,
            BagmanRealEstateTypeId = realEstate.BagmanRealEstateTypeId
        };

        // attachments
        response.Attachments.AddRange(await getAttachments(request.RealEstateValuationId, cancellationToken));

        var revDetail = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.RealEstateValudationData>(request.RealEstateValuationId, cancellationToken);
        _mapper.MapFromDataToSingle(revDetail?.Data, response);

        return response;
    }

    private async Task<List<RealEstateValuationAttachment>> getAttachments(int realEstateValuationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Attachments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == realEstateValuationId)
            .Select(t => new Contracts.RealEstateValuationAttachment
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

    private readonly Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationDetailHandler(
        RealEstateValuationServiceDbContext dbContext, 
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper mapper)
    {
        _mapper = mapper;
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
    }
}
