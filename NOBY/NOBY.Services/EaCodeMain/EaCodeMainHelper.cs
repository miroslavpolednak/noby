using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using NOBY.Infrastructure.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOBY.Services.EaCodeMain;

public interface IEaCodeMainHelper
{
    Task ValidateEaCodeMain(int eCodeMainId, CancellationToken cancellationToken);

    Task<List<int>> GetDocumentTypeIdsAccordingEaCodeMain(int eCodeMainId, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class EaCodeMainHelper : IEaCodeMainHelper
{
    private readonly ICodebookServiceClient _codebookService;

    public EaCodeMainHelper(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }

    public async Task<List<int>> GetDocumentTypeIdsAccordingEaCodeMain(int eCodeMainId, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        return documentTypes.Where(r => r.EACodeMainId == eCodeMainId)
                                    .Select(s => s.Id)
                                    .ToList();
    }

    public async Task ValidateEaCodeMain(int eCodeMainId, CancellationToken cancellationToken)
    {
        var eaCodeMains = await _codebookService.EaCodesMain(cancellationToken);
        var eaCodeMain = eaCodeMains.Find(r => r.Id == eCodeMainId) 
            ?? throw new NobyValidationException($"Specified EACodeMainId:{eCodeMainId} isn't valid");
        
        if (!eaCodeMain.IsFormIdRequested)
            throw new NobyValidationException($"Specified EACodeMainId has IsFormIdRequested == false");
    }
}
