using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;
using Google.Protobuf;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderOnlineValuation;

internal sealed class OrderOnlineValuationHandler
    : IRequestHandler<OrderOnlineValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        var deedOfOwnerships = await _dbContext
            .DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new { t.RealEstateIds })
            .ToListAsync(cancellationToken);

        var attachments = await _dbContext
            .Attachments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => t.ExternalId)
            .ToListAsync(cancellationToken);

        // realestateids
        var realEstateIds = deedOfOwnerships
            .Where(t => !string.IsNullOrEmpty(t.RealEstateIds))
            .SelectMany(t =>
            {
                return System.Text.Json.JsonSerializer.Deserialize<long[]>(t.RealEstateIds!)!;
            })
            .ToArray();

        // case detail
        var caseInstance = await _caseService.GetCaseDetail(entity.CaseId, cancellationToken);
        // klient
        var customer = await _customerService.GetCustomerDetail(caseInstance.Customer.Identity, cancellationToken);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);

        var preorderRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineMPRequestDTO
        {
            ValuationRequestId = entity.PreorderId.GetValueOrDefault(),
            DealNumber = caseInstance.Data.ContractNumber,
            ClientName = $"{customer.NaturalPerson?.FirstName} {customer.NaturalPerson?.LastName}",
            ClientEmail = customer.Contacts?.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email)?.Email?.EmailAddress,
            CremRealEstateIds = realEstateIds,
            AttachmentIds = attachments,
            EFormId = 0,
            CompanyCode = "02",
            ProductCode = "02",
            Cpm = Convert.ToInt64(currentUser.UserInfo.Cpm, CultureInfo.InvariantCulture),// nez to v ACV opravi
            Icp = Convert.ToInt64(currentUser.UserInfo.Icp, CultureInfo.InvariantCulture)
        };

        var preorderResponse = await _preorderService.OrderOnline(preorderRequest, cancellationToken);

        // ulozeni vysledku
        entity.OrderId = preorderResponse.OrderId;
        entity.ValuationSentDate = _dbContext.CisDateTime.Now;
        entity.ValuationStateId = (int)RealEstateValuationStates.Dokonceno;

        // if revaluation
        if (entity.IsRevaluationRequired)
        {
            var orderEntity = new Database.Entities.RealEstateValuationOrder
            {
                RealEstateValuationId = entity.RealEstateValuationId,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(request.Data),
                DataBin = request.Data.ToByteArray()
            };
            _dbContext.RealEstateValuationOrders.Add(orderEntity);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IPreorderServiceClient _preorderService;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly IUserServiceClient _userService;
    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerServiceClient _customerService;

    public OrderOnlineValuationHandler(
        IPreorderServiceClient preorderService,
        RealEstateValuationServiceDbContext dbContext,
        IUserServiceClient userService,
        ICaseServiceClient caseService,
        ICustomerServiceClient customerService)
    {
        _preorderService = preorderService;
        _userService = userService;
        _caseService = caseService;
        _customerService = customerService;
        _dbContext = dbContext;
    }
}
