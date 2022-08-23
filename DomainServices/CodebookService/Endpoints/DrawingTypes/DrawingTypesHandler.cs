using DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;

namespace DomainServices.CodebookService.Endpoints.DrawingTypes
{
    public class DrawingTypesHandler
        : IRequestHandler<DrawingTypesRequest, List<DrawingTypeItem>>
    {
        public Task<List<DrawingTypeItem>> Handle(DrawingTypesRequest request, CancellationToken cancellationToken)
        {
            //TODO nakesovat?
            var values = FastEnum.GetValues<CIS.Foms.Enums.DrawingTypes>()
                .Select(t => new DrawingTypeItem
                {
                    Id = (int)t,
                    EnumValue = t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? String.Empty,
                    StarbuildId = t.GetAttribute<CIS.Core.Attributes.CisStarbuildIdAttribute>()?.StarbuildId,
                })
                .ToList();
    
            return Task.FromResult(values);
        }
    }
}
