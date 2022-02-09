using CIS.Core.Enums;
using CIS.Core.Results;
using DomainServices.DocumentService.Abstraction.Interfaces;

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

        public Task<IServiceCallResult> GetDocumentsListByContractNumber(string contractNumber, IdentitySchemes mandant, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> GetDocumentsListByCustomerId(string customerId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> GetDocumentsListByRelationId(string relationId, IdentitySchemes mandant, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        Task<IServiceCallResult> IDocumentServiceAbstraction.GetDocumentStatus(string documentId, IdentitySchemes mandant, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
