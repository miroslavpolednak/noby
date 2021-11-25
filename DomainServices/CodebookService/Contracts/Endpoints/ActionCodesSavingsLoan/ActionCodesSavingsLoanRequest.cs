using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ActionCodesSavingsLoan
{
    [DataContract]
    public class ActionCodesSavingsLoanRequest : IRequest<List<GenericCodebookItem>>
    {
    }
}
