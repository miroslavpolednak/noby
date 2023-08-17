using CIS.Foms.Enums;
using DomainServices.RealEstateValuationService.Api.Services;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderStandardValuation;

internal sealed class OrderStandardValuationHandler
    : IRequestHandler<OrderStandardValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderStandardValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);

        // validace
        if (entity.OrderId.HasValue || !(new[] { (int)RealEstateValuationStates.DoplneniDokumentu, (int)RealEstateValuationStates.Rozpracovano }).Contains(entity.ValuationStateId))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.OrderCustomValidationFailed);
        }

        // detail oceneni
        var houseAndFlat = OrderAggregate.GetHouseAndFlat(entity);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);
        // info o produktu
        var (collateralAmount, loanAmount, loanDuration, loanPurpose) = await _aggregate.GetProductProperties(caseInstance.State, caseInstance.CaseId, cancellationToken);

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
            IsNotUsableTechnicalState = houseAndFlat?.PoorCondition,
            MaturityLoan = loanDuration,
            PurposeLoan = loanPurpose
        };
        if (collateralAmount.HasValue)
            orderRequest.ActualPurchasePrice = Convert.ToDouble(collateralAmount, CultureInfo.InvariantCulture);
        if (loanAmount.HasValue)
            orderRequest.LoanAmount = Convert.ToDouble(loanAmount, CultureInfo.InvariantCulture);

        entity.ValuationTypeId = (int)RealEstateValuationTypes.Standard;
        var orderResponse = await _preorderService.CreateOrder(orderRequest, cancellationToken);

        // ulozeni vysledku
        await _aggregate.SaveResults(entity, orderResponse.OrderId, RealEstateValuationStates.Probiha, request.Data, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;
    
    public OrderStandardValuationHandler(
        Services.OrderAggregate aggregate,
        IPreorderServiceClient preorderService,
        IUserServiceClient userService)
    {
        _aggregate = aggregate;
        _preorderService = preorderService;
        _userService = userService;
    }
}
