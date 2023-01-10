using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using Mandants = CIS.Infrastructure.gRPC.CisTypes.Mandants;

namespace NOBY.Api.Endpoints.Customer.Create;

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
            var createResult = await _customerService.CreateCustomer(request.ToDomainService(Mandants.Kb), cancellationToken);
            kbId = createResult.CreatedCustomerIdentity.IdentityId;
            createOk = true;
        }
        // V případě, že existoval jeden klient
        catch (CisValidationException ex) when (ex.Errors[0].Code == "11023")
        {
            _logger.LogInformation("CreateCustomer: client found {KBID}", ex.Message);
            kbId = long.Parse(ex.Message, System.Globalization.CultureInfo.InvariantCulture);
        }
        // Více klientů
        catch (CisValidationException ex) when (ex.Errors[0].Code == "11024")
        {
            _logger.LogInformation("CreateCustomer: more clients found", ex);
            throw new CisConflictException(ex.Message);
        }
        // Registry nefungují
        catch (CisValidationException ex) when (ex.Errors[0].Code == "11025")
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            return new CreateResponse { ResponseCode = "KBCM_NOT_FOUND_IN_BR" };
        }
        catch (CisValidationException ex) when (ex.Errors[0].Code == "11026")
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
        var customerKb = await _customerService.GetCustomerDetail(new Identity
        {
            IdentityId = kbId,
            IdentityScheme = Identity.Types.IdentitySchemes.Kb
        }, cancellationToken);

        // nas customer
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // update customerOnSA. Dostanu nove PartnerId
        var updateResponse = await _customerOnSAService.UpdateCustomer(customerOnSA.ToUpdateRequest(customerKb), cancellationToken);

        // vytvorit response z API
        var model = customerKb
            .ToResponseDto()
            .SetResponseCode(createOk)
            .InputDataComparison(request);

        // pokud je vse OK, zalozit customera v konsDb
        try
        {
            await _customerService.CreateCustomer(request.ToDomainService(Mandants.Mp, new Identity(updateResponse.PartnerId!.Value, IdentitySchemes.Mp), new Identity(kbId, IdentitySchemes.Kb)), cancellationToken);
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
