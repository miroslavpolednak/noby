﻿using System.Globalization;

using CIS.Foms.Enums;

using DomainServices.SalesArrangementService.Api.Handlers.Shared;


namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{

    #region Construction

    private readonly FormDataService _formDataService;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Repositories.NobyRepository _repository;
    private readonly UserService.Clients.IUserServiceClient _userService;
   

    public SendToCmpHandler(
        FormDataService formDataService,
        ILogger<SendToCmpHandler> logger,
        Repositories.NobyRepository repository,
        UserService.Clients.IUserServiceClient userService)
    {
        _formDataService = formDataService;
        _logger = logger;
        _repository = repository;
        _userService = userService;
    }

    private class DynamicFormValues
    {
        public string DocmentId { get; init; } = String.Empty;
        public string FormId { get; init; } = String.Empty;
    }

    #endregion


    private EFormType[] GetFormTypes(SalesArrangementCategories arrangementCategory)
    {
        EFormType[] formTypes;

        switch (arrangementCategory)
        {
            case SalesArrangementCategories.ProductRequest:
                formTypes = new EFormType[] { EFormType.F3601, EFormType.F3602};
                break;

            case SalesArrangementCategories.ServiceRequest:
                formTypes = new EFormType[] { EFormType.F3700 };
                break;

            default:
                throw new CisArgumentException(99999, $"Sales arrangement category #{arrangementCategory} is not supported.", nameof(arrangementCategory));
        }
        return formTypes;
    }

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellation)
    {        
        var formData = await _formDataService.LoadAndPrepare(request.SalesArrangementId, cancellation, true);
        var builder = new FormDataJsonBuilder(formData);

        _logger.LogDebug($"Parsed ProductTypeKind: {builder.ProductTypeKind} from data [ProductTypeId: {formData.ProductType?.Id}, LoanKindId: {formData.Offer?.SimulationInputs?.LoanKindId}]");

        // load user
        var user = ServiceCallResult.ResolveAndThrowIfError<UserService.Contracts.User>(await _userService.GetUser(formData.Arrangement.Created.UserId!.Value, cancellation));

        var formsToSave = GetFormTypes(formData.ArrangementCategory);

        // TODO: run in transaction ?
        for (var i = 0; i < formsToSave.Length; i++)
        {
            var formType = formsToSave[i];

            // get DocmentId, FormId
            var dynamicFormValues = await GetDynamicFormValues(cancellation);

            // build data
            var jsonData = builder.BuildJson(formType, dynamicFormValues.FormId);

            // save to DB
            await SaveForm(user, DefaultFormValues.GetInstance(formType), dynamicFormValues, formData.Arrangement.ContractNumber, jsonData, cancellation);
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

    private async Task<DynamicFormValues> GetDynamicFormValues(CancellationToken cancellation)
    {
        var count = await _repository.GetFormsCount(cancellation);

        var id = count + 1;

        var docmentId = GenerateDocmentId(id);
        var formId = GenerateFormId(id);

        return new DynamicFormValues { DocmentId = docmentId, FormId = formId };
    }


    private async Task SaveForm(UserService.Contracts.User user, DefaultFormValues defaultFormValues, DynamicFormValues dynamicFormValues, string contractNumber, string jsonData, CancellationToken cancellation)
    {
        // save to DB
        var entity = new Repositories.Entities.FormInstanceInterface()
        {
            DOKUMENT_ID = dynamicFormValues.DocmentId,
            TYP_FORMULARE = defaultFormValues.TypFormulare,
            CISLO_SMLOUVY = contractNumber,
            STATUS = 100,
            DRUH_FROMULARE = 'N',
            FORMID = dynamicFormValues.FormId,
            CPM = user.CPM ?? String.Empty,
            ICP = user.ICP ?? String.Empty,
            CREATED_AT = DateTime.Now,          // what time zone?
            HESLO_KOD = defaultFormValues.HesloKod,
            STORNOVANO = 0,
            TYP_DAT = 1,
            JSON_DATA_CLOB = jsonData
        };

        await _repository.CreateForm(entity, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.FormInstanceInterface), long.Parse(dynamicFormValues.FormId, CultureInfo.InvariantCulture));
    }

    #endregion
  
}