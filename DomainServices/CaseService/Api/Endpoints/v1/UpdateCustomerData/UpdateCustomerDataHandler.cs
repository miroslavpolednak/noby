using CIS.Infrastructure.Caching.Grpc;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateCustomerData;

internal sealed class UpdateCustomerDataHandler(
    IMediator _mediator,
    IGrpcServerResponseCache _responseCache,
    CaseServiceDbContext _dbContext)
        : IRequestHandler<UpdateCustomerDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCustomerDataRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        //TODO zkontrolovat existenci klienta?

        var customerNameChanged =
            !string.Equals(entity.Name, request.Customer.Name, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(entity.FirstNameNaturalPerson, request.Customer.FirstNameNaturalPerson, StringComparison.OrdinalIgnoreCase);

        // ulozit do DB
        entity.DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson;
        entity.Name = request.Customer.Name;
        entity.FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson;
        entity.CustomerIdentityId = request.Customer.Identity?.IdentityId;
        entity.CustomerIdentityScheme = (IdentitySchemes)Convert.ToInt32(request.Customer.Identity?.IdentityScheme, CultureInfo.InvariantCulture);
        entity.Cin = request.Customer.Cin;

        await _dbContext.SaveChangesAsync(cancellation);

        if (customerNameChanged && entity.State == (int)EnumCaseStates.InProgress)
        {
            await _mediator.Send(new NotifyStarbuildRequest { CaseId = request.CaseId }, cancellation);
        }

        await _responseCache.InvalidateEntry(nameof(GetCaseDetail), request.CaseId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
