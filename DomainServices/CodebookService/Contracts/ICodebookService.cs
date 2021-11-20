using System.ServiceModel;

namespace DomainServices.CodebookService.Contracts
{
    [ServiceContract(Name = "DomainServices.CodebookService")]
    public partial interface ICodebookService
    {
        // do tohoto interfacu primo nesahat! Pridavat nove metody je mozne v novem souboru jako partial interface.
    }
}
