using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Api.Database.Entities;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DomainServices.OfferService.Api.Endpoints.v1.CreateResponseCode;

internal sealed class CreateResponseCodeHandler(
	Database.OfferServiceDbContext _dbContext, 
	ICodebookServiceClient _codebookService)
		: IRequestHandler<CreateResponseCodeRequest, CreateResponseCodeResponse>
{
	private const string _newFq = "New_Fq";
	private const string _respCode = "Resp_Code";
	private const string _postpone = "Postpone";
	private const string _newBnk = "NewBnk";

	public async Task<CreateResponseCodeResponse> Handle(CreateResponseCodeRequest request, CancellationToken cancellationToken)
	{
		var responseCode = new ResponseCode
		{
			ResponseCodeTypeId = request.ResponseCodeTypeId,
			CaseId = request.CaseId,
			ResponseCodeCategory = (int)request.ResponseCodeCategory,
			Data = request.Data,
		};

		var responseCodes = await _codebookService.ResponseCodeTypes(cancellationToken);

		var caseIdAccountNumber = await _dbContext.CaseIdAccountNumbers.FirstOrDefaultAsync(c => c.CaseId == request.CaseId, cancellationToken)
						   ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseIdNotFoundInKonsDb);

		var applicationEvents = request.ResponseCodeCategory switch
		{
			ResponseCodeCategories.BusinessResponseCode => await CreateBussinesAppEvents(request, caseIdAccountNumber, cancellationToken),
			ResponseCodeCategories.NewFixedRatePeriod => CreateNewFixedRatePeriodAppEvents(request, caseIdAccountNumber),
			_ => throw new ArgumentException("Not supported ResponseCodeCategories")
		};

		await _dbContext.ResponseCodes.AddAsync(responseCode, cancellationToken);
		await _dbContext.ApplicationEvents.AddRangeAsync(applicationEvents, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return new CreateResponseCodeResponse
		{
			ResponseCodeId = responseCode.ResponseCodeId
		};
	}

	private static IEnumerable<ApplicationEvent> CreateNewFixedRatePeriodAppEvents(CreateResponseCodeRequest request, CaseIdAccountNumberKonstDb caseIdAccountNumber)
	{
		yield return
			new ApplicationEvent
			{
				AccountNbr = caseIdAccountNumber.AreaCodeAccountNumber,
				EventDate = DateTime.Now,
				EventType = _newFq,
				EventValue = request.Data
			};
	}

	private async Task<List<ApplicationEvent>> CreateBussinesAppEvents(CreateResponseCodeRequest request, CaseIdAccountNumberKonstDb caseIdAccountNumber, CancellationToken cancellationToken)
	{
		var responseCode = (await _codebookService.ResponseCodeTypes(cancellationToken))
		   .FirstOrDefault(t => t.Id == request.ResponseCodeTypeId);

		List<ApplicationEvent> applicationEvents =
			[
			 new ApplicationEvent
			 {
				 AccountNbr = caseIdAccountNumber.AreaCodeAccountNumber,
				 EventDate = DateTime.Now,
				 EventType = _respCode,
				 EventValue = responseCode?.Id.ToString(CultureInfo.InvariantCulture)
			 }
			];

		if (responseCode is not null && responseCode.DataType is CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode
			or CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date)
		{
			applicationEvents.Add(new ApplicationEvent
			{
				AccountNbr = caseIdAccountNumber.AreaCodeAccountNumber,
				EventDate = DateTime.Now,
				EventType = responseCode.DataType switch
				{
					CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date => _postpone,
					CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode => _newBnk,
					_ => throw new ArgumentException("Not supported ResponseCodesItemDataTypes")
				},
				EventValue = request.Data
			});
		}

		return applicationEvents;
	}
}
