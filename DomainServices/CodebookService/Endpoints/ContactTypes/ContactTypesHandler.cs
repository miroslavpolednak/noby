using DomainServices.CodebookService.Contracts.Endpoints.ContactTypes;

namespace DomainServices.CodebookService.Endpoints.ContactTypes;

public class ContactTypesHandler : IRequestHandler<ContactTypesRequest, List<ContactTypeItem>>
{
    // Pokud budes menit, zmenit i enum ContactTypes

    public Task<List<ContactTypeItem>> Handle(ContactTypesRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new List<ContactTypeItem>()
        {
            new ContactTypeItem { Id = 1, Code = "MOBIL_PRIVATE", Name = "mobil soukromý", CodeKb = "MOBILE" },
            new ContactTypeItem { Id = 2, Code = "MOBIL_WORK", Name = "mobil služební", CodeKb = "MOBILE_OTHER" },
            new ContactTypeItem { Id = 3, Code = "LANDLINE_HOME", Name = "pevná linka domů", CodeKb = "PHONE_HOME_MAIN" },
            new ContactTypeItem { Id = 5, Code = "EMAIL", Name = "e-mail", CodeKb = "EMAIL_MAIN" }
        });
}

