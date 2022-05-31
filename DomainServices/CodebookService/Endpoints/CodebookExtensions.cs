namespace DomainServices.CodebookService.Endpoints
{
    internal static class CodebookExtensions
    {

        /// <summary>
        /// Converts string with separated integers into list.
        /// </summary>
        public static List<int>? ParseIDs(this string value, string separator = ",")
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;

            return value
                    .Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToList();
        }

    }
}