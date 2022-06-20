using System.Globalization;

using DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement.Shared;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{

    #region Construction

    private readonly FormDataService _formDataService;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Repositories.NobyRepository _repository;
    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;
   

    public SendToCmpHandler(
        FormDataService formDataService,
        ILogger<SendToCmpHandler> logger,
        Repositories.NobyRepository repository,
        UserService.Abstraction.IUserServiceAbstraction userService)
    {
        _formDataService = formDataService;
        _logger = logger;
        _repository = repository;
        _userService = userService;
    }

    private class DefaultFormValues
    {
        public string? TYP_FORMULARE { get; init; }
        public string? HESLO_KOD { get; init; }
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellation)
    {
        var formData = await _formDataService.LoadAndPrepare(request.SalesArrangementId, cancellation);
        var builder = new FormDataJsonBuilder(formData);

        // load user
        var user = ServiceCallResult.ResolveAndThrowIfError<UserService.Contracts.User>(await _userService.GetUser(formData.Arrangement.Created.UserId!.Value, cancellation));

        var formsToSave = new EFormType[] { 
            EFormType.F3601, 
            EFormType.F3602 
        };

        // TODO: run in transaction ?
        for(var i = 0; i < formsToSave.Length; i++)
        {
            var formType = formsToSave[i];

            // build data
            var jsonData = builder.BuildJson(formType);

            // save to DB
            await SaveForm(user, GetDefaultFormValues(formType), formData.Arrangement.ContractNumber, jsonData, cancellation);
        }

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

    private static DefaultFormValues GetDefaultFormValues(EFormType formType)
    {
        DefaultFormValues? formValues;

        switch (formType)
        {
            case EFormType.F3601:
                formValues = new DefaultFormValues { TYP_FORMULARE = "3601A", HESLO_KOD= "608248" };
                break;

            case EFormType.F3602:
                formValues = new DefaultFormValues { TYP_FORMULARE = "3602A", HESLO_KOD = "608243" };
                break;

            default:
                throw new CisArgumentException(99999, $"Form type #{formType} is not supported.", nameof(formType));  //TODO: ErrorCode
        }

        return formValues;
    }

    private async Task SaveForm(UserService.Contracts.User user, DefaultFormValues defaultFormValues, string contractNumber, string jsonData, CancellationToken cancellation)
    {
        var count = await _repository.GetFormsCount(cancellation);

        var id = count + 1;

        var docmentId = GenerateDocmentId(id);
        var formId = GenerateFormId(id);

        // save to DB
        var entity = new Repositories.Entities.FormInstanceInterface()
        {
            DOKUMENT_ID = docmentId,
            TYP_FORMULARE = defaultFormValues.TYP_FORMULARE,
            CISLO_SMLOUVY = contractNumber,
            STATUS = 100,
            DRUH_FROMULARE = 'N',
            FORMID = formId,
            CPM = user.CPM ?? String.Empty,
            ICP = user.ICP ?? String.Empty,
            CREATED_AT = DateTime.Now,          // what time zone?
            HESLO_KOD = defaultFormValues.HESLO_KOD,
            STORNOVANO = 0,
            TYP_DAT = 1,
            JSON_DATA_CLOB = jsonData
        };

        await _repository.CreateForm(entity, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.FormInstanceInterface), long.Parse(formId, CultureInfo.InvariantCulture));
    }

    #endregion
  
}