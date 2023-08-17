using CIS.Core;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.Audit;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using FastEnumUtility;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocument;

public sealed class SignDocumentHandler : IRequestHandler<SignDocumentRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IEasClient _easClient;
    private readonly ISulmClientHelper _sulmClientHelper;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IProductServiceClient _productService;
    private readonly IAuditLogger _auditLogger;
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;

    public SignDocumentHandler(
        DocumentOnSAServiceDbContext dbContext,
        IDateTime dateTime,
        ICurrentUserAccessor currentUser,
        ISalesArrangementServiceClient salesArrangementService,
        IEasClient easClient,
        ISulmClientHelper sulmClientHelper,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSaService,
        IProductServiceClient productService,
        IAuditLogger auditLogger,
        ISalesArrangementStateManager salesArrangementStateManager)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUser = currentUser;
        _salesArrangementService = salesArrangementService;
        _easClient = easClient;
        _sulmClientHelper = sulmClientHelper;
        _householdService = householdService;
        _customerOnSAService = customerOnSaService;
        _productService = productService;
        _auditLogger = auditLogger;
        _salesArrangementStateManager = salesArrangementStateManager;
    }

    public async Task<Empty> Handle(SignDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FirstOrDefaultAsync(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value, cancellationToken);

        if (documentOnSa is null)
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId!.Value);

        if (documentOnSa.IsSigned)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AlreadySignedDocumentOnSA, request.DocumentOnSAId!.Value);

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        if (salesArrangement.State != SalesArrangementStates.InSigning.ToByte())
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);

        if (documentOnSa.IsValid == false || documentOnSa.IsSigned || documentOnSa.IsFinal)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToStartSigningOrSignInvalidDocument);

        var signatureDate = _dateTime.Now;

        UpdateDocumentOnSa(documentOnSa, signatureDate);

        await AddSignatureIfNotSetYet(documentOnSa, salesArrangement, signatureDate, cancellationToken);

        // Update Mortgage.FirstSignatureDate
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()) // 4
            await UpdateFirstSignatureDate(signatureDate, salesArrangement, cancellationToken);

        var houseHold = documentOnSa.HouseholdId.HasValue
            ? await _householdService.GetHousehold(documentOnSa.HouseholdId!.Value, cancellationToken)
            : null;
        
        // SUML call
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()) // 4 
            await SumlCall(documentOnSa, houseHold!, cancellationToken);

        // set flow switches
        if (houseHold is not null)
        {
            var flowSwitchId = houseHold.HouseholdTypeId switch
            {
                (int)HouseholdTypes.Main => FlowSwitches.Was3601MainChangedAfterSigning,
                (int)HouseholdTypes.Codebtor => FlowSwitches.Was3602CodebtorChangedAfterSigning,
                _ => throw new CisValidationException("Unsupported HouseholdType")
            };
        
            await _salesArrangementService.SetFlowSwitches(houseHold.SalesArrangementId, new()
            {
                new() { FlowSwitchId = (int)flowSwitchId, Value = false }
            }, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby007,
            "Dokument byl označen za podepsaný",
            products: new List<AuditLoggerHeaderItem>
            {
                new("case", salesArrangement.CaseId),
                new("salesArrangement", salesArrangement.SalesArrangementId),
                new("form", documentOnSa.FormId)
            }
        );
        
        // SA state
        if (salesArrangement.State == SalesArrangementStates.InSigning.ToByte())
        {
            await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);
        }
        else
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);
        }
        
        return new Empty();
    }

    private async Task UpdateFirstSignatureDate(DateTime signatureDate, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        //SalesArrangement parameters
        salesArrangement.Mortgage.FirstSignatureDate = signatureDate;
        await _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Mortgage = salesArrangement.Mortgage
        },
            cancellationToken);

        //KonsDb 
        var mortgageResponse = await _productService.GetMortgage(salesArrangement.CaseId, cancellationToken);
        mortgageResponse.Mortgage.FirstSignatureDate = signatureDate;

        await _productService.UpdateMortgage(new UpdateMortgageRequest { ProductId = salesArrangement.CaseId, Mortgage = mortgageResponse.Mortgage }, cancellationToken);
    }

    private async Task SumlCall(DocumentOnSa documentOnSa, Household houseHold, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);
        if (salesArrangement.IsProductSalesArrangement())
        {
            await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId1!.Value, cancellationToken);

            if (houseHold.CustomerOnSAId2 is not null)
            {
                await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId2.Value, cancellationToken);
            }
        }
    }

    private async Task SumlCallForSpecifiedCustomer(int customerOnSaId, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);
        var kbIdentity = customerOnSa.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        await _sulmClientHelper.StartUse(kbIdentity.IdentityId, ISulmClient.PurposeMLAP, cancellationToken);
    }

    private async Task AddSignatureIfNotSetYet(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, DateTime signatureDate, CancellationToken cancellationToken)
    {
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()
            && await _dbContext.DocumentOnSa.Where(d => d.SalesArrangementId == documentOnSa.SalesArrangementId).AllAsync(r => r.IsSigned == false, cancellationToken))
        {
            var result = await _easClient.AddFirstSignatureDate((int)salesArrangement.CaseId, signatureDate, cancellationToken);

            if (result is not null && result.CommonValue != 0)
            {
                throw new CisValidationException(ErrorCodeMapper.EasAddFirstSignatureDateReturnedErrorState, $"Eas AddFirstSignatureDate returned error state: {result.CommonValue} with message: {result.CommonText}");
            }
        }
    }

    private void UpdateDocumentOnSa(DocumentOnSa documentOnSa, DateTime signatureDate)
    {
        documentOnSa.IsSigned = true;
        documentOnSa.SignatureDateTime = signatureDate;
        documentOnSa.SignatureConfirmedBy = _currentUser.User?.Id;
    }
}
