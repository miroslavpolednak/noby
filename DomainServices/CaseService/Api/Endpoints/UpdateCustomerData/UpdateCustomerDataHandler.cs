using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCustomerData;

internal sealed class UpdateCustomerDataHandler
    : IRequestHandler<UpdateCustomerDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCustomerDataRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        //TODO zkontrolovat existenci klienta?

        var customerNameChanged =
            !string.Equals(entity.Name, request.Customer.Name, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(entity.FirstNameNaturalPerson, request.Customer.FirstNameNaturalPerson, StringComparison.OrdinalIgnoreCase);
        
        // ulozit do DB
        entity.DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson;
        entity.Name = request.Customer.Name;
        entity.FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson;
        entity.CustomerIdentityId = request.Customer.Identity?.IdentityId;
        entity.CustomerIdentityScheme = (CIS.Foms.Enums.IdentitySchemes)Convert.ToInt32(request.Customer.Identity?.IdentityScheme, CultureInfo.InvariantCulture);
        entity.Cin = request.Customer.Cin;

        await _dbContext.SaveChangesAsync(cancellation);

        if (customerNameChanged)
        {
            await _mediator.Send(new NotifyStarbuildRequest { CaseId = request.CaseId  }, cancellation);    
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IMediator _mediator;
    private readonly CaseServiceDbContext _dbContext;

    public UpdateCustomerDataHandler(
        IMediator mediator,
        CaseServiceDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
}
