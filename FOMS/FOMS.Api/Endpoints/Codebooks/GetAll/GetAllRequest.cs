namespace FOMS.Api.Endpoints.Codebooks.Dto;

internal sealed class GetAllRequest : IRequest<GetAllResponse>
{
    public List<string> CodebookCodes { get; init; }

    public GetAllRequest(string q)
    {
        if (string.IsNullOrEmpty(q))
            throw new ArgumentNullException("q", "Specify which codebooks are to be returned");

        CodebookCodes = q.Split(",").Select(t => t.ToLower()).ToList();

        // zkontrolovat duplikaty
        var duplicates = CodebookCodes.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
        if (duplicates.Any())
            throw new Exception($"Codebooks {string.Join(",", duplicates)} duplicated in request");
    }
}
