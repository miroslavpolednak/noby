using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.LegacyContracts.Resend;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Resend;

internal sealed class ResendHandler
    : IRequestHandler<ResendRequest>
{
    public async Task Handle(ResendRequest request, CancellationToken cancellationToken)
    {
        _userAdapterService.CheckResendNotificationsAccess();

        // resend jen na MPSS emaily
        var email = await _dbContext.EmailResults
            .Where(t => t.Id == request.NotificationId && t.SenderType == LegacyContracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.State == LegacyContracts.Result.Dto.NotificationState.Invalid || t.State == LegacyContracts.Result.Dto.NotificationState.Unsent || t.State == LegacyContracts.Result.Dto.NotificationState.Error)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CisNotFoundException(ErrorCodeMapper.ResultNotFound, $"Result with id = '{request.NotificationId}' not found."); ;

        email.State = LegacyContracts.Result.Dto.NotificationState.InProgress;
        email.Resend = true;

        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete From SentNotification Where Id = {email.Id}", default);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly NotificationDbContext _dbContext;
    private readonly IUserAdapterService _userAdapterService;

    public ResendHandler(NotificationDbContext dbContext, IUserAdapterService userAdapterService)
    {
        _dbContext = dbContext;
        _userAdapterService = userAdapterService;
    }
}
