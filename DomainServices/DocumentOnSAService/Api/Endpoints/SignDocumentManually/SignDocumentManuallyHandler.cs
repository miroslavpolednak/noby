﻿using CIS.Core;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Globalization;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocumentManually;

public sealed class SignDocumentManuallyHandler : IRequestHandler<SignDocumentManuallyRequest, Empty>
{
    private const string ManualSigningMethodCode = "PHYSICAL";
    /// <summary>
    /// Form 3601
    /// </summary>
    private const int DocumentType = 4;

    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly IEasClient _easClient;
    private readonly ISulmClientHelper _sulmClientHelper;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly IProductServiceClient _productServiceClient;

    public SignDocumentManuallyHandler(
        DocumentOnSAServiceDbContext dbContext,
        IDateTime dateTime,
        ICurrentUserAccessor currentUser,
        ISalesArrangementServiceClient arrangementServiceClient,
        IEasClient easClient,
        ISulmClientHelper sulmClientHelper,
        ICodebookServiceClient codebookServiceClient,
        IHouseholdServiceClient householdClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        IProductServiceClient productServiceClient)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUser = currentUser;
        _arrangementServiceClient = arrangementServiceClient;
        _easClient = easClient;
        _sulmClientHelper = sulmClientHelper;
        _codebookServiceClient = codebookServiceClient;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _productServiceClient = productServiceClient;
    }

    public async Task<Empty> Handle(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FirstOrDefaultAsync(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value, cancellationToken);

        if (documentOnSa is null)
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId!.Value);

        if (documentOnSa.SignatureMethodCode!.ToUpper(CultureInfo.InvariantCulture) != ManualSigningMethodCode || documentOnSa.IsSigned)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToSignDocumentOnSA, request.DocumentOnSAId!.Value);

        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);


        //ToDo temporary disabled, until DM will do change in SA validation
        //if (salesArrangement.State != (int)SalesArrangementStates.InSigning)
        //    throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToStartSigningOrSignInvalidSalesArrangementState);

        //if (documentOnSa.IsValid == false || documentOnSa.IsSigned || documentOnSa.IsFinal)
        //    throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToStartSigningOrSignInvalidDocument); 

        UpdateDocumentOnSa(documentOnSa);

        await AddSignatureIfNotSetYet(documentOnSa, salesArrangement, cancellationToken);

        // Update Mortgage.FirstSignatureDate
        await UpdateMortgageFirstSignatureDate(documentOnSa, salesArrangement, cancellationToken);

        // SUML call
        await SumlCall(documentOnSa, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }

    private async Task UpdateMortgageFirstSignatureDate(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var mortgageResponse = await _productServiceClient.GetMortgage(salesArrangement.CaseId, cancellationToken);
        mortgageResponse.Mortgage.FirstSignatureDate = _dateTime.Now;
        await _productServiceClient.UpdateMortgage(new() { ProductId = salesArrangement.CaseId, Mortgage = mortgageResponse.Mortgage }, cancellationToken);
    }

    private async Task SumlCall(DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);
        var salesArrangementTypes = await _codebookServiceClient.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);

        if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
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

    private async Task AddSignatureIfNotSetYet(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        if (documentOnSa.DocumentTypeId == DocumentType
            && await _dbContext.DocumentOnSa.Where(d => d.SalesArrangementId == documentOnSa.SalesArrangementId).AllAsync(r => r.IsSigned == false, cancellationToken))
        {
            var result = await _easClient.AddFirstSignatureDate((int)salesArrangement.CaseId, _dateTime.Now.Date, cancellationToken);

            if (result is not null && result.CommonValue != 0)
            {
                throw new CisValidationException(ErrorCodeMapper.EasAddFirstSignatureDateReturnedErrorState, $"Eas AddFirstSignatureDate returned error state: {result.CommonValue} with message: {result.CommonText}");
            }
        }
    }

    private void UpdateDocumentOnSa(DocumentOnSa documentOnSa)
    {
        documentOnSa.IsSigned = true;
        documentOnSa.SignatureDateTime = _dateTime.Now;
        documentOnSa.SignatureConfirmedBy = _currentUser.User?.Id;
    }
}
