namespace DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskTypes
{
    [DataContract]
    public class WorkflowTaskTypeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int? CategoryId { get; set; }
    }
}