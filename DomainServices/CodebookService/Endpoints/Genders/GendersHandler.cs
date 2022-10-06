using DomainServices.CodebookService.Contracts.Endpoints.Genders;

namespace DomainServices.CodebookService.Endpoints.Genders;

public class GendersHandler
    : IRequestHandler<GendersRequest, List<GenderItem>>
{
    public Task<List<GenderItem>> Handle(GendersRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.Genders>()
            .Select(t => new GenderItem
            {
                Id = (int)t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                MpHomeCode = t,
                KonsDBCode = (int)t,
                KbCmCode = MapGenderToCode(t, "M", "F"),
                StarBuildJsonCode = MapGenderToCode(t, "M", "Z")
                //KbCmCode = t == CIS.Foms.Enums.Genders.Female ? "F" : "M",
                //StarBuildJsonCode = t == CIS.Foms.Enums.Genders.Female ? "Z" : "M"
            })
            .ToList();

        return Task.FromResult(values);
    }

    private static string MapGenderToCode(CIS.Foms.Enums.Genders value, string maleCode, string femaleCode) => value switch
    {
        CIS.Foms.Enums.Genders.Male => maleCode,
        CIS.Foms.Enums.Genders.Female => femaleCode,
        _ => string.Empty
    };
}