namespace FOMS.Api.Endpoints.Codebooks.Dto;

internal sealed class GetAllRequest : IRequest<List<Dto.GetAllResponseItem>>
{
    public List<(string Original, string Key)> CodebookCodes { get; init; }

    public GetAllRequest(string q)
    {
        if (string.IsNullOrEmpty(q))
            throw new ArgumentNullException("q", "Specify which codebooks are to be returned");

        CodebookCodes = q.Split(",").Select(t => (Original: t, Key: t.ToLower())).ToList();

        // zkontrolovat duplikaty
        var duplicates = CodebookCodes.GroupBy(x => x.Key)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
        if (duplicates.Any())
            throw new CisException(ErrorCodes.CodebookDuplicatedInQueryParam, $"Codebooks {string.Join(",", duplicates)} duplicated in request");
    }
}
