using DomainServices.UserService.Api.Dto;

namespace DomainServices.UserService.Api.Endpoints.v1.GetUserBasicInfo;

internal sealed class GetUserBasicInfoHandler(IConnectionProvider _db)
        : IRequestHandler<Contracts.GetUserBasicInfoRequest, Contracts.GetUserBasicInfoResponse>
{
    public async Task<Contracts.GetUserBasicInfoResponse> Handle(Contracts.GetUserBasicInfoRequest request, CancellationToken cancellationToken)
    {
        // vytahnout info o uzivateli z DB
        var userInfo = await _db.ExecuteDapperStoredProcedureFirstOrDefaultAsync<GetUserDisplayNameDto>(
            "[dbo].[getUserDisplayName]",
            new { v33id = request.UserId },
            cancellationToken);

        if (string.IsNullOrEmpty(userInfo?.DisplayName))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.UserId}");
        }

        // vytvorit finalni model
        var model = new Contracts.GetUserBasicInfoResponse
        {
            DisplayName = userInfo!.DisplayName
        };

        return model;
    }
}
