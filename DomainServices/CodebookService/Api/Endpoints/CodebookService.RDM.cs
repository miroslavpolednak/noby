using DomainServices.CodebookService.Contracts.v1;
using System.Globalization;

namespace DomainServices.CodebookService.Api.Endpoints;

internal partial class CodebookService
{
    public override Task<ProfessionCategoriesResponse> ProfessionCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var mappings = _db.SelfDb.GetRdmMappings("MAP_CB_CmEpProfessionCategory_CB_CmEpProfession");
            var items = _db.SelfDb.GetRdmItems("CB_CmEpProfessionCategory");
            var sb = _db.GetDynamicList(nameof(ProfessionCategories), 1);
            var extensions = _db.GetDynamicList(nameof(ProfessionCategories), 2);

            var finalItems = items.Select(item =>
            {
                var obj = new ProfessionCategoriesResponse.Types.ProfessionCategoryItem
                {
                    IsValid = item.IsValid,
                    Id = Convert.ToInt32(item.Code, CultureInfo.InvariantCulture),
                    Name = item.Properties["Name"]
                };

                // rdm-sb mapping
                var foundMappings = mappings
                    .Where(t => t.Source == item.Code)
                    .Select(t =>
                    {
                        var f = sb.FirstOrDefault(x => x.RdmCode == Convert.ToInt32(t.Target, CultureInfo.InvariantCulture));
                        return f is null ? -1 : Convert.ToInt32(f.Id);
                    })
                    .Where(t => t >= 0)
                    .Cast<int>()
                    .ToArray();
                obj.ProfessionTypeIds.AddRange(foundMappings);

                // nase ext
                var ext = extensions.FirstOrDefault(x => x.ProfessionCategoryId == obj.Id);
                if (ext is not null)
                {
                    if (!string.IsNullOrEmpty(ext.IncomeMainTypeAMLIds))
                    {
                        obj.IncomeMainTypeAMLIds.AddRange(((string)ext.IncomeMainTypeAMLIds).ParseIDs());
                    }
                }

                return obj;
            });

            return (new ProfessionCategoriesResponse()).AddItems(finalItems);
        });

    public override Task<SigningMethodsForNaturalPersonResponse> SigningMethodsForNaturalPerson(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.SelfDb.GetRdmItems("CB_StandardMethodOfArrAcceptanceByNPType");
            var extensions = _db.GetDynamicList(nameof(SigningMethodsForNaturalPerson));

            var finalItems = items.Select(item =>
            {
                var obj = new SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem
                {
                    IsValid = item.IsValid,
                    Name = item.Properties["Name"],
                    Description = item.Properties["Description"],
                    Order = item.SortOrder,
                    Code = item.Code,
                    StarbuildEnumId = extensions.FirstOrDefault(x => x.Code == item.Code)?.StarbuildEnumId ?? 2
                };

                return obj;
            });

            return (new SigningMethodsForNaturalPersonResponse()).AddItems(finalItems);
        });
}
