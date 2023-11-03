using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

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
            OnlineDisqualified = realEstate.IsOnlineDisqualified,
            ValuationResultCurrentPrice = realEstate.ValuationResultCurrentPrice,
            ValuationResultFuturePrice = realEstate.ValuationResultFuturePrice,
            RealEstateSubtypeId = realEstate.RealEstateSubtypeId,
            ACVRealEstateTypeId = realEstate.ACVRealEstateTypeId,
            BagmanRealEstateTypeId = realEstate.BagmanRealEstateTypeId,
            LoanPurposeDetails = realEstate.LoanPurposeDetailsBin is null ? null : LoanPurposeDetailsObject.Parser.ParseFrom(realEstate.LoanPurposeDetailsBin)
        };

        // attachments
        response.Attachments.AddRange(await getAttachments(request.RealEstateValuationId, cancellationToken));

        // documents
        if (!string.IsNullOrEmpty(realEstate.Documents))
        {
            response.Documents.Add(System.Text.Json.JsonSerializer.Deserialize<RealEstateValuationDocument>(realEstate.Documents));
        }

        // specific details
        if (realEstate.SpecificDetailBin is not null)
        {
            switch (Helpers.GetRealEstateType(response))
            {
                case SharedTypes.Enums.RealEstateTypes.Hf:
                case SharedTypes.Enums.RealEstateTypes.Hff:
                    response.HouseAndFlatDetails = SpecificDetailHouseAndFlatObject.Parser.ParseFrom(realEstate.SpecificDetailBin);
                    break;

                case SharedTypes.Enums.RealEstateTypes.P:
                    response.ParcelDetails = SpecificDetailParcelObject.Parser.ParseFrom(realEstate.SpecificDetailBin);
                    break;
            }
        }

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

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationDetailHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
