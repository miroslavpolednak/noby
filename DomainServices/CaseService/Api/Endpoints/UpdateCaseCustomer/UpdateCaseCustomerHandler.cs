using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseCustomer;

internal sealed class UpdateCaseCustomerHandler
    : IRequestHandler<UpdateCaseCustomerRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseCustomerRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw new CisNotFoundException(13000, "Case", request.CaseId);
        //TODO zkontrolovat existenci klienta?

        // ulozit do DB
        entity.DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson;
        entity.Name = request.Customer.Name;
        entity.FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson;
        entity.CustomerIdentityId = request.Customer.Identity?.IdentityId;
        entity.CustomerIdentityScheme = (CIS.Foms.Enums.IdentitySchemes)Convert.ToInt32(request.Customer.Identity?.IdentityScheme, CultureInfo.InvariantCulture);
        entity.Cin = request.Customer.Cin;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CaseServiceDbContext _dbContext;

    public UpdateCaseCustomerHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
