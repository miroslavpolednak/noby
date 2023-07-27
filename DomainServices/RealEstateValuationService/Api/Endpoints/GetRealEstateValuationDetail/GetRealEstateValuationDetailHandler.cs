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

        // attachments
        var attachments = await _dbContext.Attachments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new Contracts.RealEstateValuationAttachment
            {
                RealEstateValuationAttachmentId = t.RealEstateValuationAttachmentId,
                Title = t.Title,
                FileName = t.FileName,
                ExternalId = t.ExternalId,
                AcvAttachmentCategoryId = t.AcvAttachmentCategoryId
            })
            .ToListAsync(cancellationToken);

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
            ValuationResultCurrentPrice = realEstate.ValuationResultCurrentPrice,
            ValuationResultFuturePrice = realEstate.ValuationResultFuturePrice,
            RealEstateSubtypeId = realEstate.RealEstateSubtypeId,
            ACVRealEstateTypeId = realEstate.ACVRealEstateTypeId,
            LoanPurposeDetails = realEstate.LoanPurposeDetailsBin is null ? null : LoanPurposeDetailsObject.Parser.ParseFrom(realEstate.LoanPurposeDetailsBin)
        };
        response.Attachments.AddRange(attachments);

        if (realEstate.SpecificDetailBin is not null)
        {
            switch (Helpers.GetRealEstateType(response))
            {
                case CIS.Foms.Types.Enums.RealEstateTypes.Hf:
                case CIS.Foms.Types.Enums.RealEstateTypes.Hff:
                    response.HouseAndFlatDetails = SpecificDetailHouseAndFlatObject.Parser.ParseFrom(realEstate.SpecificDetailBin);
                    break;

                case CIS.Foms.Types.Enums.RealEstateTypes.P:
                    response.ParcelDetails = SpecificDetailParcelObject.Parser.ParseFrom(realEstate.SpecificDetailBin);
                    break;
            }
        }

        return response;
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationDetailHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
