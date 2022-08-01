using DomainServices.CodebookService.Contracts.Endpoints.LoanInterestRateAnnouncedTypes;

namespace DomainServices.CodebookService.Endpoints.LoanInterestRateAnnouncedTypes
{
    public class LoanInterestRateAnnouncedTypesHandler
        : IRequestHandler<LoanInterestRateAnnouncedTypesRequest, List<LoanInterestRateAnnouncedTypeItem>>
    {

        public Task<List<LoanInterestRateAnnouncedTypeItem>> Handle(LoanInterestRateAnnouncedTypesRequest request, CancellationToken cancellationToken)
        {
            //TODO nakesovat?
            var values = FastEnum.GetValues<CIS.Foms.Enums.LoanInterestRateAnnouncedTypes>()
                .Select(t => new LoanInterestRateAnnouncedTypeItem()
                {
                    Id = (int)t,
                    Code = t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? String.Empty,
                })
                .ToList();

            return Task.FromResult(values);
        }

    }
}
