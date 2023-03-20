namespace DomainServices.CodebookService.Contracts.Endpoints.ProfessionCategories;

[DataContract]
public class ProfessionCategoryItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    public bool IsValid { get; set; }


    [DataMember(Order = 4)]
    public List<int> ProfessionTypeIds { get; set; }


    [DataMember(Order = 5)]
    public List<int> IncomeMainTypeAMLIds { get; set; }
}