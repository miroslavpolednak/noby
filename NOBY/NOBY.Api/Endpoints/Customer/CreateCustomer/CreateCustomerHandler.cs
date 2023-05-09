﻿using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using Mandants = CIS.Infrastructure.gRPC.CisTypes.Mandants;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

internal sealed class CreateCustomerHandler
    : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        // vytvorit customera v CM
        long kbId;
        bool isVerified = false;
        try
        {
            var createResult = await _customerService.CreateCustomer(request.ToDomainService(Mandants.Kb), cancellationToken);
            kbId = createResult.CreatedCustomerIdentity.IdentityId;
            isVerified = !request.HardCreate;
        }
        // V případě, že existoval jeden klient
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11023")
        {
            _logger.LogInformation("CreateCustomer: client found {KBID}", ex.Message);
            kbId = long.Parse(ex.Message, System.Globalization.CultureInfo.InvariantCulture);
        }
        // Více klientů
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11024")
        {
            _logger.LogInformation("CreateCustomer: more clients found", ex);
            throw new CisConflictException(90006, ex.Message);
        }
        // Registry nefungují
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11025")
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            throw new CisValidationException(90007, "KBCM_NOT_FOUND_IN_BR");
        }
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11026")
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            throw new CisException(90008, "KBCM_UNAVAILABLE_BR");
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
            .ToResponseDto(isVerified)
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

    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;

    public CreateCustomerHandler(
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        ICustomerServiceClient customerService,
        ILogger<CreateCustomerHandler> logger)
    {
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _logger = logger;
    }
}