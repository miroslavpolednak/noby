﻿using CIS.Core;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.Audit;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
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
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly IEasClient _easClient;
    private readonly ISulmClientHelper _sulmClientHelper;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly IProductServiceClient _productServiceClient;
    private readonly IAuditLogger _auditLogger;
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;

    public SignDocumentHandler(
        DocumentOnSAServiceDbContext dbContext,
        IDateTime dateTime,
        ICurrentUserAccessor currentUser,
        ISalesArrangementServiceClient arrangementServiceClient,
        IEasClient easClient,
        ISulmClientHelper sulmClientHelper,
        IHouseholdServiceClient householdClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        IProductServiceClient productServiceClient,
        IAuditLogger auditLogger,
        ISalesArrangementStateManager salesArrangementStateManager)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUser = currentUser;
        _arrangementServiceClient = arrangementServiceClient;
        _easClient = easClient;
        _sulmClientHelper = sulmClientHelper;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _productServiceClient = productServiceClient;
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

        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

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

        // SUML call
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()) // 4 
            await SumlCall(documentOnSa, cancellationToken);

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
        await _arrangementServiceClient.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Mortgage = salesArrangement.Mortgage
        },
            cancellationToken);

        //KonsDb 
        var mortgageResponse = await _productServiceClient.GetMortgage(salesArrangement.CaseId, cancellationToken);
        mortgageResponse.Mortgage.FirstSignatureDate = signatureDate;

        await _productServiceClient.UpdateMortgage(new UpdateMortgageRequest { ProductId = salesArrangement.CaseId, Mortgage = mortgageResponse.Mortgage }, cancellationToken);
    }

    private async Task SumlCall(DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);
        if (salesArrangement.IsProductSalesArrangement())
        {
            var houseHold = await _householdClient.GetHousehold(documentOnSa.HouseholdId!.Value, cancellationToken);
            await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId1!.Value, cancellationToken);

            if (houseHold.CustomerOnSAId2 is not null)
            {
                await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId2.Value, cancellationToken);
            }
        }
    }

    private async Task SumlCallForSpecifiedCustomer(int customerOnSaId, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAServiceClient.GetCustomer(customerOnSaId, cancellationToken);
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
