
using Grpc.Core;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.UserService.Abstraction;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class ValidateSalesArrangementHandler
    : IRequestHandler<Dto.ValidateSalesArrangementMediatrRequest, ValidateSalesArrangementResponse>
{

    #region Construction

    private readonly SalesArrangement.Shared.FormDataService _formDataService;
    private readonly ILogger<ValidateSalesArrangementHandler> _logger;
    private readonly Eas.IEasClient _easClient;

    public ValidateSalesArrangementHandler(
        SalesArrangement.Shared.FormDataService formDataService,
        ILogger<ValidateSalesArrangementHandler> logger,
        Eas.IEasClient easClient)
    {
        _formDataService = formDataService;
        _logger = logger;
        _easClient = easClient;
    }

    #endregion

    public async Task<ValidateSalesArrangementResponse> Handle(Dto.ValidateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        var formData = await _formDataService.LoadAndPrepare(request.SalesArrangementId, cancellation);
        var builder = new SalesArrangement.Shared.FormDataJsonBuilder(formData);

        var actualDate = DateTime.Now.Date;
        var jsonData = builder.BuildJson3601001();

        var checkFormData = new Eas.EasWrapper.CheckFormData()
        {
            formular_id = 3601001,
            cislo_smlouvy = formData.Arrangement.ContractNumber,
            // dokument_id = "9876543210",          //??? dokument_id je nepovinné, to neposílej
            datum_prijeti = actualDate,             //??? datum prijeti dej v D1.2 aktuální datum
            data = jsonData,
        };

        //var checkFormData = SalesArrangement.FormDataJsonBuilder.BuildSampleFormData(3601001);

        var checkFormResult = ResolveCheckForm(await _easClient.CheckFormV2(checkFormData));

        return ResultToResponse(checkFormResult);
    }

    private static Eas.CheckFormV2.Response ResolveCheckForm(IServiceCallResult result) =>
      result switch
      {
          SuccessfulServiceCallResult<Eas.CheckFormV2.Response> r => r.Model,
          ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
          _ => throw new NotImplementedException()
      };

    private static ValidateSalesArrangementResponse ResultToResponse(Eas.CheckFormV2.Response result)
    {
        ValidationMessage ToMessage(string parameter, Eas.CheckFormV2.Error e)
        {
            return new ValidationMessage
            {
                Parameter = parameter,
                Value = e.Value,
                Code = e.ErrorCode,
                Message = e.ErrorMessage,
                AdditionalInformation = e.AdditionalInformation
            };
        };

        var response = new ValidateSalesArrangementResponse { };

        if (result.Detail != null)
        {
            var parameters = result.Detail.errors.Select(i => i.Key);

            var errors = parameters?.SelectMany(p => result.Detail.errors[p].Where(i => i.Severity).Select(e => ToMessage(p, e)));
            var warnings = parameters?.SelectMany(p => result.Detail.errors[p].Where(i => !i.Severity).Select(e => ToMessage(p, e)));

            response.Errors.AddRange(errors);
            response.Warnings.AddRange(warnings);
        }

        return response;
    }
}

