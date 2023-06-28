namespace NOBY.Api.Endpoints.Codebooks.GetAll;

internal sealed class GetAllRequest : IRequest<List<GetAllResponseItem>>
{
    public List<(string Original, string Key)> CodebookCodes { get; init; }

    public GetAllRequest(string q)
    {
        if (string.IsNullOrEmpty(q))
            throw new ArgumentNullException(nameof(q), "Specify which codebooks are to be returned");

        CodebookCodes = q.Split(",").Select(t => (Original: t, Key: t.ToLower(System.Globalization.CultureInfo.InvariantCulture))).ToList();

        // zkontrolovat duplikaty
        var duplicates = CodebookCodes.GroupBy(x => x.Key)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
        if (duplicates.Any())
            throw new NobyValidationException($"Codebooks {string.Join(",", duplicates)} duplicated in request");
    }
}
