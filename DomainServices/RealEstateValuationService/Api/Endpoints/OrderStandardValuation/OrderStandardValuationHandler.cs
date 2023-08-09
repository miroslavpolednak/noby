using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderStandardValuation;

internal sealed class OrderStandardValuationHandler
    : IRequestHandler<OrderStandardValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderStandardValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);
        // detail oceneni
        var houseAndFlat = getHouseAndFlat(entity);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);

        var orderRequest = new ExternalServices.PreorderService.V1.Contracts.StandardOrderRequestDTO
        {
            CompanyCode = "02",
            ProductCode = "02",
            ProductOwner = "02",
            DealNumber = caseInstance.Data.ContractNumber,
            ContactPersonName = $"{currentUser.UserInfo.FirstName} {currentUser.UserInfo.LastName}",
            ContactPersonEmail = currentUser.UserInfo.Email,
            ContactPersonTel = currentUser.UserInfo.PhoneNumber,
            Cpm = Convert.ToInt64(currentUser.UserInfo.Cpm, CultureInfo.InvariantCulture),// nez to v ACV opravi
            Icp = Convert.ToInt64(currentUser.UserInfo.Icp, CultureInfo.InvariantCulture),
            LocalSurveyPerson = $"{request.Data?.FirstName} {request.Data?.LastName}",
            LocalSurveyEmail = request.Data?.Email,
            LocalSurveyPhone = $"{request.Data?.PhoneIDC}{request.Data?.PhoneNumber}",
            LocalSurveyFunction = request.Data?.RealEstateValuationLocalSurveyFunctionCode,
            BagmanRealEstateTypeId = entity.BagmanRealEstateTypeId,
            RealEstateTypeId = entity.ACVRealEstateTypeId,
            CremRealEstateIds = realEstateIds,
            LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO(),
            AttachmentIds = attachments,
            EFormId = 0,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            OwnershipLimitations = houseAndFlat?.OwnershipRestricted,
            IsCellarFlat = houseAndFlat?.FlatOnlyDetails?.Basement,
            IsNotUsableTechnicalState = houseAndFlat?.PoorCondition
        };

        if (caseInstance.State == (int)CaseStates.InProgress)
        {
            var (_, offerId) = await _salesArrangementService.GetProductSalesArrangement(caseInstance.CaseId, cancellationToken);
            var offer = await _offerService.GetMortgageOfferDetail(offerId!.Value, cancellationToken);

            orderRequest.ActualPurchasePrice = Convert.ToDouble((decimal)offer.SimulationInputs.CollateralAmount);
            orderRequest.MaturityLoan = offer.SimulationInputs.LoanDuration;
            orderRequest.PurposeLoan = await getLoanPurpose(offer.SimulationInputs.LoanPurposes?.FirstOrDefault()?.LoanPurposeId);
            orderRequest.LoanAmount = Convert.ToDouble((decimal)offer.SimulationInputs.LoanAmount);
        }
        else
        {
            var mortgage = await _productService.GetMortgage(caseInstance.CaseId, cancellationToken);
            
            orderRequest.PurposeLoan = await getLoanPurpose(mortgage.Mortgage.LoanPurposes?.FirstOrDefault()?.LoanPurposeId);
            orderRequest.LoanAmount = mortgage.Mortgage.LoanPaymentAmount;
        }
        
        var orderResponse = await _preorderService.OrderStandard(orderRequest, cancellationToken);

        // ulozeni vysledku
        await _aggregate.SaveResults(entity, orderResponse.OrderId, request.Data, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task<string?> getLoanPurpose(int? loanPurposeId)
    {
        return (await _codebookService.LoanPurposes()).FirstOrDefault(t => t.Id == loanPurposeId)?.AcvId;
    }

    private static SpecificDetailHouseAndFlatObject? getHouseAndFlat(Database.Entities.RealEstateValuation entity)
    {
        if (entity.SpecificDetailBin is not null)
        {
            switch (Helpers.GetRealEstateType(entity))
            {
                case CIS.Foms.Types.Enums.RealEstateTypes.Hf:
                case CIS.Foms.Types.Enums.RealEstateTypes.Hff:
                    return SpecificDetailHouseAndFlatObject.Parser.ParseFrom(entity.SpecificDetailBin);
            }
        }
        return null;
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;
    private readonly IProductServiceClient _productService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;

    public OrderStandardValuationHandler(
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ICodebookServiceClient codebookService,
        IProductServiceClient productService,
        Services.OrderAggregate aggregate,
        IPreorderServiceClient preorderService,
        IUserServiceClient userService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _codebookService = codebookService;
        _productService = productService;
        _aggregate = aggregate;
        _preorderService = preorderService;
        _userService = userService;
    }
}
