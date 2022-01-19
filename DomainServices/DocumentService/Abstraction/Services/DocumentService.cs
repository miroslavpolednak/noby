using CIS.Core;
using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.DocumentService.Abstraction.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace DomainServices.DocumentService.Abstraction.Services
{
    internal class DocumentService : IDocumentServiceAbstraction
    {
        public Task<IServiceCallResult> GetDocument(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> GetDocumentsListByCaseId(Int32 caseId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
