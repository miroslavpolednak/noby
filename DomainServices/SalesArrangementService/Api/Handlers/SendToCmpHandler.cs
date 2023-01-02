using System.Globalization;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.EasForms;
using DomainServices.SalesArrangementService.Api.Repositories.Entities;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Repositories.NobyRepository _repository;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly Forms.FormsService _formsService;

    public SendToCmpHandler(
        ILogger<SendToCmpHandler> logger,
        Repositories.NobyRepository repository,
        UserService.Clients.IUserServiceClient userService,
        Forms.FormsService formsService)
    {
        _logger = logger;
        _repository = repository;
        _userService = userService;
        _formsService = formsService;
    }

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellationToken)
    {
        var category = await _formsService.LoadSalesArrangementCategory(request.SalesArrangementId, cancellationToken);

        var easForm = await GetEasForm(request.SalesArrangementId, category, cancellationToken);

        //await SaveForms(easForm, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task<IEasForm<IEasFormData>> GetEasForm(int salesArrangementId, SalesArrangementCategories category, CancellationToken cancellationToken)
    {
        return category switch
        {
            SalesArrangementCategories.ProductRequest => await ProcessProductRequest(salesArrangementId, cancellationToken),
            SalesArrangementCategories.ServiceRequest => await _formsService.LoadServiceForm(salesArrangementId),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<IEasForm<IEasFormData>> ProcessProductRequest(int salesArrangementId, CancellationToken cancellationToken)
    {
        var productForm = await _formsService.LoadProductForm(salesArrangementId);

        await _formsService.UpdateContractNumber(productForm.FormData, cancellationToken);
        await _formsService.AddFirstSignatureDate(productForm.FormData);
        await _formsService.CallSulm(productForm.FormData, cancellationToken);

        return productForm;
    }

    private async Task SaveForms(IEasForm<IEasFormData> easForm, CancellationToken cancellationToken)
    {
        // load user
        var user = await _userService.GetUser(easForm.FormData.SalesArrangement.Created.UserId!.Value, cancellationToken);

        var dynamicValues = await GetDynamicValuesRange(cancellationToken, easForm.FormData is IServiceFormData ? 1 : 3);

        // map to entities
        var entities = easForm.BuildForms(dynamicValues).Select(f =>
        {
            var defaultValues = CIS.InternalServices.DataAggregator.EasForms.FormData.DefaultValues.Create(f.FormType);

            return new FormInstanceInterface
            {
                DOKUMENT_ID = f.DynamicValues!.DocumentId,
                TYP_FORMULARE = defaultValues.FormType,
                CISLO_SMLOUVY = easForm.FormData.SalesArrangement.ContractNumber,
                STATUS = 100,
                DRUH_FROMULARE = 'N',
                FORMID = f.DynamicValues.FormId,
                CPM = user.CPM ?? string.Empty,
                ICP = user.ICP ?? string.Empty,
                CREATED_AT = DateTime.Now,          // what time zone?
                HESLO_KOD = defaultValues.PasswordCode,
                STORNOVANO = 0,
                TYP_DAT = 1,
                JSON_DATA_CLOB = f.Json
            };
        }).ToList();

        await _repository.CreateForms(entities, cancellationToken);

        var documentIds = entities.Select(e => e.DOKUMENT_ID);

        _logger.LogInformation($"Entities {nameof(FormInstanceInterface)} created [ContractNumber: {easForm.FormData.SalesArrangement}, DokumentIds: {string.Join(", ", documentIds)} ]");
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