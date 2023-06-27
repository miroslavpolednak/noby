using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailHandler
    : IRequestHandler<UpdateRealEstateValuationDetailRequest, Empty>
{
    public async Task<Empty> Handle(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var realEstate = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // general detail
        realEstate.Address = request.Address;
        realEstate.IsLoanRealEstate = request.IsLoanRealEstate;
        realEstate.RealEstateStateId = request.RealEstateValuationId;

        // detail
        await updateDetails(request, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }

    private async Task updateDetails(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        // dotahnout z DB - muze a nemusi existovat
        var details = await _dbContext.RealEstateValuationDetails
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken);
        // pokud neexistuje, zalozit novy
        if (details is null)
        {
            details = new Database.Entities.RealEstateValuationDetail
            {
                RealEstateValuationId = request.RealEstateValuationId
            };
            _dbContext.RealEstateValuationDetails.Add(details);
        }

        details.RealEstateSubtypeId = request.RealEstateSubtypeId;

        if (request.LoanPurposeDetails is null)
        {
            details.LoanPurposeDetails = null!;
            details.LoanPurposeDetailsBin = null!;
        }
        else
        {
            details.LoanPurposeDetails = Newtonsoft.Json.JsonConvert.SerializeObject(request.LoanPurposeDetails);
            details.LoanPurposeDetailsBin = request.LoanPurposeDetails.ToByteArray();
        }

        switch (request.SpecificDetailCase)
        {
            case UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.HouseAndFlatDetails:
                details.SpecificDetail = Newtonsoft.Json.JsonConvert.SerializeObject(request.HouseAndFlatDetails);
                details.SpecificDetailBin = request.HouseAndFlatDetails.ToByteArray();
                break;

            case UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.ParcelDetails:
                details.SpecificDetail = Newtonsoft.Json.JsonConvert.SerializeObject(request.ParcelDetails);
                details.SpecificDetailBin = request.ParcelDetails.ToByteArray();
                break;

            default:
                details.SpecificDetail = null!;
                details.SpecificDetailBin = null!;
                break;
        }
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public UpdateRealEstateValuationDetailHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
