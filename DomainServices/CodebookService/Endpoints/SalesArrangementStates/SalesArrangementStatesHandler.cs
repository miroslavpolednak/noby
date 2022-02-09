using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementStates
{
    public class SalesArrangementStatesHandler
        : IRequestHandler<SalesArrangementStatesRequest, List<SalesArrangementStateItem>>
    {
        public Task<List<SalesArrangementStateItem>> Handle(SalesArrangementStatesRequest request, CancellationToken cancellationToken)
        {
            //TODO nakesovat?
            var values = Enum.GetValues<CIS.Core.Enums.SalesArrangementStates>()
                .Select(t => new SalesArrangementStateItem
                {
                    Id = (int)t,
                    Value = t,
                    Name = t.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description,
                    IsDefault = t == CIS.Core.Enums.SalesArrangementStates.InProcess
                })
                .ToList();
    
            return Task.FromResult(values);
        }
    }
}
