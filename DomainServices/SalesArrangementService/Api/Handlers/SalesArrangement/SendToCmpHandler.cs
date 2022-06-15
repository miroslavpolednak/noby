using System.Globalization;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{

    #region Construction

    private readonly SalesArrangement.Shared.FormDataService _formDataService;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Repositories.NobyRepository _repository;
    private readonly Eas.IEasClient _easClient;

    public SendToCmpHandler(
        SalesArrangement.Shared.FormDataService formDataService,
        ILogger<SendToCmpHandler> logger,
        Repositories.NobyRepository repository,
        Eas.IEasClient easClient)
    {
        _formDataService = formDataService;
        _logger = logger;
        _repository = repository;
        _easClient = easClient;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellation)
    {
        var formData = await _formDataService.LoadAndPrepare(request.SalesArrangementId, cancellation);
        var builder = new SalesArrangement.Shared.FormDataJsonBuilder(formData);

        var jsonData = builder.BuildJson3601001();

        // save form to DB
        await SaveForm(formData.Arrangement.ContractNumber, jsonData, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    #region Form data
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

    private async Task SaveForm(string contractNumber, string jsonData, CancellationToken cancellation)
    {
        var count = await _repository.GetFormsCount(cancellation);

        var id = count + 1;

        var docmentId = GenerateDocmentId(id);
        var formId = GenerateFormId(id);

        // save to DB
        var entity = new Repositories.Entities.FormInstanceInterface()
        {
            DOKUMENT_ID = docmentId,
            TYP_FORMULARE = "3601A",
            CISLO_SMLOUVY = contractNumber,
            STATUS = 100,
            DRUH_FROMULARE = 'N',
            FORMID = formId,
            CPM = String.Empty,                 // add from user
            ICP = String.Empty,                 // add from user
            CREATED_AT = DateTime.Now,          // what time zone?
            HESLO_KOD = "600248",
            STORNOVANO = 0,
            TYP_DAT = 1,
            JSON_DATA_CLOB = jsonData
        };

        await _repository.CreateForm(entity, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.FormInstanceInterface), long.Parse(formId, CultureInfo.InvariantCulture));
    }


    #endregion
}