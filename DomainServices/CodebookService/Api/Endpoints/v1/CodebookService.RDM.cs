using DomainServices.CodebookService.Contracts.v1;
using System.Globalization;

namespace DomainServices.CodebookService.Api.Endpoints.v1;

internal partial class CodebookService
{
    public override Task<ProfessionCategoriesResponse> ProfessionCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var mappings1 = _db.SelfDb.GetRdmMappings("MAP_CB_CmEpProfessionCategory_CB_CmEpProfession");
			var mappings2 = _db.SelfDb.GetRdmMappings("CB_SourceOfEarningsVsProfessionCategory");
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
                var foundMappings = mappings1
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
                    obj.IsValidNoby = ext.IsValidNoby;

                    if (!string.IsNullOrEmpty(ext.IncomeMainTypeAMLIds))
                    {
                        obj.IncomeMainTypeAMLIds.AddRange(((string)ext.IncomeMainTypeAMLIds).ParseIDs());
                    }
                }

                return obj;
            });

            return (new ProfessionCategoriesResponse()).AddItems(finalItems);
        });

	public override Task<CountryCodePhoneIdcResponse> CountryCodePhoneIdc(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
		=> Helpers.GetItems(() =>
		{
			var items = _db.SelfDb.GetRdmItems("CB_CmCoPhoneIdc");
			
			var finalItems = items
                .Select(item => new CountryCodePhoneIdcResponse.Types.CountryCodePhoneIdcItem
				{
					IsValid = item.IsValid,
					Name = item.Properties["COUNTRY_CODE"],
					Id = item.Code,
                    Idc = item.Properties["PHONE_IDC"],
                    IsDefault = item.Properties["COUNTRY_CODE"] == "CZ",
                    IsPriority = item.Properties["IS_PRIORITY"] == "1"
				})
                .OrderBy(t => t.Name);

			return (new CountryCodePhoneIdcResponse()).AddItems(finalItems);
		});

	public override Task<GenericCodebookResponse> IdentificationSubjectMethods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
		=> Helpers.GetItems(() =>
		{
			var items = _db.SelfDb.GetRdmItems("CB_IdentificationMethodType");

			var finalItems = items
				.Select(item => new GenericCodebookResponse.Types.GenericCodebookItem
				{
					IsValid = item.IsValid,
					Name = item.Properties["Name"],
					Id = Convert.ToInt32(item.Code, CultureInfo.InvariantCulture)
				})
				.Where(t => t.Id is 1 or 3 or 8)
				.OrderBy(t => t.Id);

			return (new GenericCodebookResponse()).AddItems(finalItems);
		});

	public override Task<SigningMethodsForNaturalPersonResponse> SigningMethodsForNaturalPerson(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.SelfDb.GetRdmItems("CB_StandardMethodOfArrAcceptanceByNPType");
            var extensions = _db.GetDynamicList(nameof(SigningMethodsForNaturalPerson));

            var finalItems = items
                .Select(item => new SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem
                {
                    IsValid = item.IsValid,
                    Name = item.Properties["Name"],
                    Description = item.Properties["Description"],
                    Order = item.SortOrder,
                    Code = item.Code,
                    StarbuildEnumId = extensions.FirstOrDefault(x => x.Code == item.Code)?.StarbuildEnumId ?? 2
                });

            return (new SigningMethodsForNaturalPersonResponse()).AddItems(finalItems);
        });

    public override Task<ResponseCodeTypesResponse> ResponseCodeTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
		=> Helpers.GetItems(() =>
		{
			var items = _db.SelfDb.GetRdmItems("CB_HyporetenceResponse");

			var finalItems = items
				.Select(item => new ResponseCodeTypesResponse.Types.ResponseCodeTypesItem
				{
					IsValid = item.IsValid,
					Name = item.Properties["Name"],
					Id = Convert.ToInt32(item.Code, CultureInfo.InvariantCulture),
                    IsAvailableForRefixation = item.Properties["CampaignType"] == "Ref",
					IsAvailableForRetention = item.Properties["CampaignType"] == "Ret",
                    MandantId = Convert.ToInt32(item.Properties["Mandant"], CultureInfo.InvariantCulture),
					DataType = item.Properties["Meta"] switch
                    {
                        "10" => ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode,
                        "01" => ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date,
						_ => ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.String
					}
				});

			return (new ResponseCodeTypesResponse()).AddItems(finalItems);
		});

	public override Task<TinFormatsByCountryResponse> TinFormatsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
		=> Helpers.GetItems(() =>
		{
			var items = _db.SelfDb.GetRdmItems("CB_CmTrTinFormat");

			var finalItems = items
				.Select(item => new TinFormatsByCountryResponse.Types.TinFormatsByCountryItem
				{
					IsValid = item.IsValid,
					Id = Convert.ToInt32(item.Code, CultureInfo.InvariantCulture),
                    CountryCode = item.Properties["country_code"],
                    IsForFo = item.Properties["is_for_foo"] == "true",
                    RegularExpression = item.Properties["regular_expression"],
                    Tooltip = item.Properties["tooltip"]
				})
				.OrderBy(t => t.Id);

			return (new TinFormatsByCountryResponse()).AddItems(finalItems);
		});

	public override Task<TinNoFillReasonsByCountryResponse> TinNoFillReasonsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
		=> Helpers.GetItems(() =>
		{
			var items = _db.SelfDb.GetRdmItems("CB_CmTrTinCountry");

			var finalItems = items
				.Select(item => new TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem
				{
					IsValid = item.IsValid,
					Id = item.Code,
					IsTinMandatory = item.Properties["is_tin_mandatory"] == "true",
					ReasonForBlankTin = item.Properties["reason_for_blank_tin"]
				})
				.OrderBy(t => t.Id);

			return (new TinNoFillReasonsByCountryResponse()).AddItems(finalItems);
		});
}
