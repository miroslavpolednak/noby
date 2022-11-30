using System.Globalization;

using CIS.Foms.Enums;

using DomainServices.SalesArrangementService.Api.Handlers.Forms;
using DomainServices.SalesArrangementService.Api.Repositories.Entities;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{

    #region Construction

    private readonly FormsService _formsService;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Repositories.NobyRepository _repository;
    private readonly UserService.Clients.IUserServiceClient _userService;
   
    public SendToCmpHandler(
        FormsService formsService,
        ILogger<SendToCmpHandler> logger,
        Repositories.NobyRepository repository,
        UserService.Clients.IUserServiceClient userService)
    {
        _formsService = formsService;
        _logger = logger;
        _repository = repository;
        _userService = userService;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellation)
    {
        // load arrangement
        var arrangement = await _formsService.LoadArrangement(request.SalesArrangementId, cancellation);

        // build forms
        var forms = await GetForms(arrangement, cancellation);

        // save to DB
        await SaveForms(arrangement, forms, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task<List<Form>> GetForms(Contracts.SalesArrangement arrangement, CancellationToken cancellation)
    {
        var arrangementCategory = await _formsService.LoadArrangementCategory(arrangement, cancellation);

        List<Form>? forms;

        switch (arrangementCategory)
        {
            case SalesArrangementCategories.ProductRequest:
                forms = await ProcessProductRequest(arrangement, cancellation);
                break;

            case SalesArrangementCategories.ServiceRequest:
                forms = await ProcessServiceRequest(arrangement, cancellation);
                break;

            default:
                forms = new List<Form>();
                break;
        }

        return forms;
    }

    private async Task<List<Form>> ProcessProductRequest(Contracts.SalesArrangement arrangement, CancellationToken cancellation)
    {
        var formData = await _formsService.LoadProductFormData(arrangement, cancellation);

        FormsService.CheckFormData(formData);

        await _formsService.SetContractNumber(formData, cancellation);
        await _formsService.AddFirstSignatureDate(formData);
        await _formsService.CallSulm(formData, cancellation);

        var dynamicValuesRange = await GetDynamicValuesRange(cancellation, 3);

        var builder = new JsonBuilder();

        _logger.LogDebug($"Parsed ProductTypeKind: {builder.ProductTypeKind} from data [ProductTypeId: {formData.ProductType?.Id}, LoanKindId: {formData.Offer?.SimulationInputs?.LoanKindId}]");

        return builder.BuildForms(formData, dynamicValuesRange);
    }

    private async Task<List<Form>> ProcessServiceRequest(Contracts.SalesArrangement arrangement, CancellationToken cancellation)
    {
        var formData = await _formsService.LoadServiceFormData(arrangement, cancellation);

        FormsService.CheckFormData(formData);

        var dynamicValuesRange = await GetDynamicValuesRange(cancellation, 1);

        var builder = new JsonBuilder();

        return builder.BuildForms(formData, dynamicValuesRange);
    }


    #region Dynamic Form Values

    private static string GenerateDocmentId(int id, int length = 30)
    {
        var prefix = "KBHNB";
        var sId = id.ToString(CultureInfo.InvariantCulture).PadLeft(length - prefix.Length, '0');
        return prefix + sId;
    }

    private static string GenerateFormId(int id, int length = 13)
    {
        // Numeric(13) ... nedoplňovat nulama!
        var prefix = "1";
        var suffix = "01";
        var sId = id.ToString(CultureInfo.InvariantCulture).PadLeft(length - suffix.Length - prefix.Length, '0');
        return prefix + sId + suffix;
    }

    private async Task<List<DynamicValues>> GetDynamicValuesRange(CancellationToken cancellation, int count = 3)
    {
        var formsCount = await _repository.GetFormsCount(cancellation);

        return Enumerable.Range(formsCount + 1, count).Select(id => new DynamicValues
        {
            DocmentId = GenerateDocmentId(id),
            FormId = GenerateFormId(id)
        }).ToList();
    }

    #endregion


    private async Task SaveForms(Contracts.SalesArrangement arrangement, List<Form> forms, CancellationToken cancellation)
    {
        // load user
        var user = await _userService.GetUser(arrangement.Created.UserId!.Value, cancellation);

        // map to entities
        var entities = forms.Select(f => 
        {
            var defaultValues = DefaultValues.GetInstance(f.FormType);

            return new FormInstanceInterface()
            {
                DOKUMENT_ID = f.DynamicValues!.DocmentId,
                TYP_FORMULARE = defaultValues.TypFormulare,
                CISLO_SMLOUVY = arrangement.ContractNumber,
                STATUS = 100,
                DRUH_FROMULARE = 'N',
                FORMID = f.DynamicValues.FormId,
                CPM = user.CPM ?? String.Empty,
                ICP = user.ICP ?? String.Empty,
                CREATED_AT = DateTime.Now,          // what time zone?
                HESLO_KOD = defaultValues.HesloKod,
                STORNOVANO = 0,
                TYP_DAT = 1,
                JSON_DATA_CLOB = f.JSON
            };
        }).ToList();

        await _repository.CreateForms(entities, cancellation);

        var dokumentIds = entities.Select(e => e.DOKUMENT_ID);

        _logger.LogInformation($"Entities {nameof(FormInstanceInterface)} created [ContractNumber: {arrangement.ContractNumber}, DokumentIds: {string.Join(", ", dokumentIds)} ]");
    }
}