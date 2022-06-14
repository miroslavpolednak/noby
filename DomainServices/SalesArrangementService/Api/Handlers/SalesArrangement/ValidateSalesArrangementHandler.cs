using System.Globalization;
using System.Text.Json;

using Grpc.Core;
using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.CodebookService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.UserService.Contracts;

using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.UserService.Abstraction;

using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class ValidateSalesArrangementHandler
    : IRequestHandler<Dto.ValidateSalesArrangementMediatrRequest, ValidateSalesArrangementResponse>
{
    #region Construction

    private static readonly string StringJoinSeparator = ",";

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly IUserServiceAbstraction _userService;

    private readonly Repositories.NobyRepository _repository;
    private readonly ILogger<ValidateSalesArrangementHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly IMediator _mediator;

    public ValidateSalesArrangementHandler(
        ICodebookServiceAbstraction codebookService,
        ICaseServiceAbstraction caseService,
        IOfferServiceAbstraction offerService,
        ICustomerServiceAbstraction customerService,
        IUserServiceAbstraction userService,
        Repositories.NobyRepository repository,
        ILogger<ValidateSalesArrangementHandler> logger,
        Eas.IEasClient easClient,
        IMediator mediator)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _offerService = offerService;
        _customerService = customerService;
        _userService = userService;
        _repository = repository;
        _logger = logger;
        _easClient = easClient;
        _mediator = mediator;
    }

    #endregion

    public async Task<ValidateSalesArrangementResponse> Handle(Dto.ValidateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        var formData = SalesArrangement.FormDataBuilder.BuildSampleFormData(3601001);

        var checkFormResult = ResolveCheckForm(await _easClient.CheckFormV2(formData));

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

        var parameters = result.Detail.errors.Select(i => i.Key);

        var errors = parameters.SelectMany(p => result.Detail.errors[p].Where(i => i.Severity).Select(e => ToMessage(p, e)));
        var warnings = parameters.SelectMany(p => result.Detail.errors[p].Where(i => !i.Severity).Select(e => ToMessage(p, e)));

        var response = new ValidateSalesArrangementResponse { };

        response.Errors.AddRange(errors);
        response.Warnings.AddRange(warnings);

        return response;
    }
}

