using DomainServices.CustomerService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.Create;

internal sealed class CreateHandler
    : IRequestHandler<CreateRequest, CreateResponse>
{
    public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        // vytvorit customera v CM
        long kbId;
        bool createOk = false;
        try
        {
            var createResult = ServiceCallResult.ResolveAndThrowIfError<_Cust.CreateCustomerResponse>(await _customerService.CreateCustomer(request.ToDomainService(new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb
            }), cancellationToken));
            kbId = createResult.CreatedCustomerIdentity.IdentityId;
            createOk = true;
        }
        // V případě, že existoval jeden klient
        catch (CisArgumentException ex) when (ex.ExceptionCode == 11023)
        {
            _logger.LogInformation("CreateCustomer: client found {KBID}", ex.Message);
            kbId = long.Parse(ex.Message, System.Globalization.CultureInfo.InvariantCulture);
        }
        // Více klientů
        catch (CisArgumentException ex) when (ex.ExceptionCode == 11024)
        {
            _logger.LogInformation("CreateCustomer: more clients found", ex);
            throw new CisConflictException(ex.Message);
        }
        // Registry nefungují
        catch (BaseCisException ex) when (ex.ExceptionCode == 11025)
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            return new CreateResponse { ResponseCode = "KBCM_NOT_FOUND_IN_BR" };
        }
        catch (BaseCisException ex) when (ex.ExceptionCode == 11026)
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            return new CreateResponse { ResponseCode = "KBCM_UNAVAILABLE_BR" };
        }
        catch
        {
            // rethrow? nevim co tady...
            throw;
        }

        // KB customer
        var customerKb = ServiceCallResult.ResolveAndThrowIfError<_Cust.CustomerDetailResponse>(await _customerService.GetCustomerDetail(new CIS.Infrastructure.gRPC.CisTypes.Identity
        {
            IdentityId = kbId,
            IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb
        }, cancellationToken));

        // nas customer
        var customerOnSA = ServiceCallResult.ResolveAndThrowIfError<_HO.CustomerOnSA>(await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken));

        // update customerOnSA. Dostanu nove PartnerId
        var updateResponse = ServiceCallResult.ResolveAndThrowIfError<_HO.UpdateCustomerResponse>(await _customerOnSAService.UpdateCustomer(customerOnSA.ToUpdateRequest(customerKb), cancellationToken));

        // vytvorit response z API
        var model = customerKb
            .ToResponseDto()
            .SetResponseCode(createOk)
            .InputDataComparison(request);

        // pokud je vse OK, zalozit customera v konsDb
        try
        {
            await _customerService.CreateCustomer(request.ToDomainService(new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityId = updateResponse.PartnerId!.Value,
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp
            }), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Can not create customer in KonsDB", ex);
        }

        return model;
    }

    private readonly ILogger<CreateHandler> _logger;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;

    public CreateHandler(
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        ICustomerServiceClient customerService,
        ILogger<CreateHandler> logger)
    {
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _logger = logger;
    }
}
