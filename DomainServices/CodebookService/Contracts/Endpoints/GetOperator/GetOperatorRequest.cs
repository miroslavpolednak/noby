using System.ComponentModel.DataAnnotations;

namespace DomainServices.CodebookService.Contracts.Endpoints.GetOperator;

[DataContract]
public class GetOperatorRequest : IRequest<GetOperatorItem>
{
    [Required]
    [DataMember(Order = 1)]
    public string PerformerLogin { get; set; }
}
