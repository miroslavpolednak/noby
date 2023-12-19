using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Resend;
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
            .Where(t => t.Id == request.NotificationId && t.SenderType == Contracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.State == Contracts.Result.Dto.NotificationState.Invalid || t.State == Contracts.Result.Dto.NotificationState.Unsent || t.State == Contracts.Result.Dto.NotificationState.Error)
            .FirstAsync(cancellationToken)
            ?? throw new CisNotFoundException(ErrorHandling.ErrorCodeMapper.ResultNotFound, $"Result with id = '{request.NotificationId}' not found."); ;

        email.State = Contracts.Result.Dto.NotificationState.InProgress;
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
