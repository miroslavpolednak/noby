namespace DomainServices.UserService.Api.Endpoints.v1.GetUserMortgageSpecialist;

internal sealed class GetUserMortgageSpecialistHandler(IConnectionProvider _db)
	: IRequestHandler<Contracts.GetUserMortgageSpecialistRequest, Contracts.GetUserMortgageSpecialistResponse>
{
	public async Task<Contracts.GetUserMortgageSpecialistResponse> Handle(Contracts.GetUserMortgageSpecialistRequest request, CancellationToken cancellationToken)
	{
		var userInfo = await _db.ExecuteDapperStoredProcedureFirstOrDefaultAsync<dynamic>(
			"[dbo].[getUserPHU]",
			new { v33id = request.UserId },
			cancellationToken);

		if (string.IsNullOrEmpty(userInfo?.firstname))
		{
			throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.UserId}");
		}

		return new Contracts.GetUserMortgageSpecialistResponse
		{
			Firstname = userInfo!.firstname,
			Lastname = userInfo.surname,
			Email = userInfo.email,
			Phone = userInfo.phone,
			MortgageCenterAdditionalId = userInfo.mortgageCenterAdditionalId,
			MortgageCenterId = userInfo.mortgageCenterId
		};
	}
}
