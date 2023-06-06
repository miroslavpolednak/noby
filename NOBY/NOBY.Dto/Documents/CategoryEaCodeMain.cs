namespace NOBY.Dto.Documents;

public class CategoryEaCodeMain
{
    public string Name { get; set; } = null!;

    public int DocumentCountInCategory { get; set; }

    public IReadOnlyCollection<int?> EaCodeMainIdList { get; set; } = null!;
}
