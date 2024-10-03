using DomainServices.CaseService.Clients.v1;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.UserService.Clients.v1;
using Google.Protobuf;
using NOBY.Services.DocumentHelper;
using NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;
using System.Runtime.CompilerServices;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.CreateIncome;

internal sealed class CreateIncomeHandler(
    ICustomerOnSAServiceClient _customerService,
    FlowSwitchAtLeastOneIncomeMainHouseholdService _flowSwitchMainHouseholdService,
    ICaseServiceClient _caseService,
    IUserServiceClient _userService,
    IDocumentHelperServiceOld _documentHelper,
    ICustomerOnSAServiceClient _customerOnSAService,
    SharedComponents.Storage.ITempStorage _tempFileManager,
    IDocumentArchiveServiceClient _documentArchiveService)
        : IRequestHandler<CustomerIncomeCreateIncomeRequest, int>
{
    public async Task<int> Handle(CustomerIncomeCreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var model = new _HO.CreateIncomeRequest
        {
            CustomerOnSAId = request.CustomerOnSAId!.Value,
            IncomeTypeId = (int)request.IncomeTypeId,
            BaseData = new _HO.IncomeBaseData
            {
                CurrencyCode = request.CurrencyCode,
                Sum = request.Sum,
            }
        };

        var documentIds = await SaveAttachments(request.CustomerOnSAId!.Value, request.Attachments ?? [], cancellationToken).ToListAsync(cancellationToken);
        model.BaseData.IncomeDocumentsId.AddRange(documentIds);

        // detail prijmu
        switch (request.IncomeTypeId)
        {
            case EnumIncomeTypes.Employement when request.Data?.Employment is not null:
                model.Employement = request.Data.Employment.ToDomainServiceRequest();
                break;

            case EnumIncomeTypes.Other when request.Data?.Other is not null:
                model.Other = request.Data.Other.ToDomainServiceRequest();
                break;

            case EnumIncomeTypes.Entrepreneur when request.Data?.Entrepreneur is not null:
                model.Entrepreneur = request.Data.Entrepreneur.ToDomainServiceRequest();
                break;

            case EnumIncomeTypes.Rent:
                // RENT nema zadna data
                model.Rent = new _HO.IncomeDataRent();
                break;

            default:
                throw new NotImplementedException($"IncomeType {request.IncomeTypeId} cast to domain service is not implemented");
        }

        int incomeId = await _customerService.CreateIncome(model, cancellationToken);

        await _flowSwitchMainHouseholdService.SetFlowSwitchByCustomerOnSAId(request.CustomerOnSAId.Value, cancellationToken: cancellationToken);

        return incomeId;
    }

    private async IAsyncEnumerable<string> SaveAttachments(int customerOnSaId, List<SharedTypesDocumentInformation> attachments, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var caseId = (await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken)).CaseId;
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);

        var user = await _userService.GetCurrentUser(cancellationToken);
        var authorUserLogin = _documentHelper.GetAuthorUserLoginForDocumentUpload(user);

        foreach (var documentInformation in attachments.Where(a => a.Guid.HasValue).GroupBy(a => a.Guid).Select(a => a.First()))
        {
            var fileMetadata = await _tempFileManager.GetMetadata(documentInformation.Guid!.Value, cancellationToken);
            var file = await _tempFileManager.GetContent(documentInformation.Guid!.Value, cancellationToken);

            var documentId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);

            var uploadRequest = MapRequest(file, fileMetadata.FileName, documentId, caseId, caseDetail.Data.ContractNumber, documentInformation, authorUserLogin);

            await _documentArchiveService.UploadDocument(uploadRequest, cancellationToken);

            yield return documentId;
        }
    }

    private static UploadDocumentRequest MapRequest(
        byte[] file,
        string fileName,
        string documentId,
        long caseId,
        string contractNumber,
        SharedTypesDocumentInformation documentInformation,
        string authorUserLogin)
    {
        return new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(file),
            Metadata = new()
            {
                CaseId = caseId,
                DocumentId = documentId,
                EaCodeMainId = documentInformation.EaCodeMainId,
                Filename = fileName,
                AuthorUserLogin = authorUserLogin,
                CreatedOn = DateTime.UtcNow.Date,
                Description = documentInformation.Description ?? string.Empty,
                FormId = string.Empty,
                ContractNumber = contractNumber
            },
            NotifyStarBuild = false
        };
    }
}
