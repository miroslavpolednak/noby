using System.Globalization;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal class SendToCmpHandler : IRequestHandler<SendToCmpRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Database.NobyRepository _repository;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly Services.Forms.FormsService _formsService;

    public SendToCmpHandler(
        ILogger<SendToCmpHandler> logger,
        Database.NobyRepository repository,
        UserService.Clients.IUserServiceClient userService,
        Services.Forms.FormsService formsService)
    {
        _logger = logger;
        _repository = repository;
        _userService = userService;
        _formsService = formsService;
    }

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var category = await _formsService.LoadSalesArrangementCategory(salesArrangement, cancellationToken);

        var easFormResponse = await GetEasForm(salesArrangement, category, cancellationToken);

        await SaveForms(easFormResponse, salesArrangement, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task<GetEasFormResponse> GetEasForm(SalesArrangement salesArrangement, SalesArrangementCategories category, CancellationToken cancellationToken)
    {
        return category switch
        {
            SalesArrangementCategories.ProductRequest => await ProcessProductRequest(salesArrangement, cancellationToken),
            SalesArrangementCategories.ServiceRequest => await ProcessServiceRequest(salesArrangement.SalesArrangementId, cancellationToken),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<GetEasFormResponse> ProcessProductRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await GetDynamicValuesRange(cancellationToken, 3);

        var response = await _formsService.LoadProductForm(salesArrangement, dynamicValues, cancellationToken);

        await _formsService.UpdateContractNumber(salesArrangement, response.Product, cancellationToken);
        await _formsService.AddFirstSignatureDate(salesArrangement.CaseId);
        await _formsService.CallSulm(response.Product, cancellationToken);

        return response;
    }

    private async Task<GetEasFormResponse> ProcessServiceRequest(int salesArrangementId, CancellationToken cancellationToken)
    {
        var dynamicValues = await GetDynamicValuesRange(cancellationToken, 1);

        return await _formsService.LoadServiceForm(salesArrangementId, dynamicValues, cancellationToken);
    }

    private async Task SaveForms(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        // load user
        var user = await _userService.GetUser(salesArrangement.Created.UserId!.Value, cancellationToken);

        // map to entities
        var entities = easFormResponse.Forms.Select(f => new Database.Entities.FormInstanceInterface
        {
            DOKUMENT_ID = f.DynamicFormValues!.DocumentId, //Mock
            TYP_FORMULARE = f.DefaultValues.FormType,
            CISLO_SMLOUVY = salesArrangement.ContractNumber,
            STATUS = 100,
            DRUH_FROMULARE = 'N',
            FORMID = f.DynamicFormValues.FormId, //Mock
            CPM = user.CPM ?? string.Empty,
            ICP = user.ICP ?? string.Empty,
            CREATED_AT = DateTime.Now, // what time zone?
            HESLO_KOD = f.DefaultValues.PasswordCode,
            STORNOVANO = 0,
            TYP_DAT = 1,
            JSON_DATA_CLOB = f.Json
        }).ToList();

        await _repository.CreateForms(entities, cancellationToken);

        var documentIds = entities.Select(e => e.DOKUMENT_ID);

        _logger.LogInformation($"Entities {nameof(Database.Entities.FormInstanceInterface)} created [ContractNumber: {salesArrangement}, DokumentIds: {string.Join(", ", documentIds)} ]");
    }

    private async Task<List<DynamicFormValues>> GetDynamicValuesRange(CancellationToken cancellationToken, int count = 3)
    {
        var formsCount = await _repository.GetFormsCount(cancellationToken);

        return Enumerable.Range(formsCount + 1, count).Select(id => new DynamicFormValues
        {
            DocumentId = GenerateDocumentId(id),
            FormId = GenerateFormId(id)
        }).ToList();
    }

    private static string GenerateDocumentId(int id, int length = 30)
    {
        const string prefix = "KBHNB";

        var sId = id.ToString(CultureInfo.InvariantCulture).PadLeft(length - prefix.Length, '0');

        return prefix + sId;
    }

    private static string GenerateFormId(int id, int length = 13)
    {
        // Numeric(13) ... nedoplňovat nulama!
        const string prefix = "1";
        const string suffix = "01";

        var sId = id.ToString(CultureInfo.InvariantCulture).PadLeft(length - suffix.Length - prefix.Length, '0');

        return prefix + sId + suffix;
    }
}