﻿using _SA = DomainServices.SalesArrangementService.Contracts;
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
            var createResult = ServiceCallResult.ResolveAndThrowIfError<_Cust.CreateCustomerResponse>(await _customerService.CreateCustomer(request.ToDomainService(), cancellationToken));
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
        // Nebyl klient nalezen
        catch (BaseCisException ex) when ((ex.ExceptionCode == 11025 || ex.ExceptionCode == 11026) && request.HardCreate)
        {
            // tady potrebuju zjistit kbid
            throw new NotImplementedException();
        }
        catch (BaseCisException ex) when ((ex.ExceptionCode == 11025 || ex.ExceptionCode == 11026) && !request.HardCreate)
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            throw new CisValidationException(ex.Message);
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
        var customerOnSA = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken));

        // update customerOnSA. Dostanu nove PartnerId
        var updateResponse = ServiceCallResult.ResolveAndThrowIfError<_SA.UpdateCustomerResponse>(await _customerOnSAService.UpdateCustomer(customerOnSA.ToUpdateRequest(customerKb), cancellationToken));

        // vytvorit response z API
        var model = customerKb.ToResponseDto();
        if (createOk || request.HardCreate)
        {
            model.InputDataDifferent = true;
            model.ResponseCode = "KBCM_CREATED";
        }
        else
        {
            model.ResponseCode = "KBCM_IDENTIFIED";
        }

        // pokud je vse OK, zalozit customera v konsDb
        try
        {
            await _customerService.CreateCustomer(request.ToDomainService(), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Can not create customer in KonsDB", ex);
        }

        return model;
    }

    private readonly ILogger<CreateHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public CreateHandler(
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSAService,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        ILogger<CreateHandler> logger)
    {
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _logger = logger;
    }
}
