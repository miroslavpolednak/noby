﻿using DomainServices.RealEstateValuationService.Api.Database;
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
            .Select(Mappers.RealEstateDetail())
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        var details = await _dbContext.RealEstateValuationDetails
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new { t.RealEstateSubtypeId, t.LoanPurposeDetailsBin, t.SpecificDetailBin })
            .FirstOrDefaultAsync(cancellationToken);

        var response = new RealEstateValuationDetail
        {
            RealEstateSubtypeId = details?.RealEstateSubtypeId,
            RealEstateValuationGeneralDetails = realEstate,
            LoanPurposeDetails = details?.LoanPurposeDetailsBin is null ? null : LoanPurposeDetailsObject.Parser.ParseFrom(details.LoanPurposeDetailsBin)
        };

        if (details?.SpecificDetailBin is not null)
        {
            switch (Helpers.GetRealEstateType(realEstate))
            {
                case CIS.Foms.Types.Enums.RealEstateTypes.Hf:
                case CIS.Foms.Types.Enums.RealEstateTypes.Hff:
                    response.HouseAndFlatDetails = SpecificDetailHouseAndFlatObject.Parser.ParseFrom(details.SpecificDetailBin);
                    break;

                case CIS.Foms.Types.Enums.RealEstateTypes.P:
                    response.ParcelDetails = SpecificDetailParcelObject.Parser.ParseFrom(details.SpecificDetailBin);
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
