using CIS.Core.Results;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class SalesArrangementServiceWrapper : IServiceWrapper
{
    private readonly ISalesArrangementServiceClients _salesArrangementService;

    public SalesArrangementServiceWrapper(ISalesArrangementServiceClients salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.SalesArrangementId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId));

        var result = await _salesArrangementService.GetSalesArrangement(input.SalesArrangementId.Value, cancellationToken);
        
        data.SalesArrangement = ServiceCallResult.ResolveAndThrowIfError<SalesArrangement>(result);

        //TODO: Mock
        data.SalesArrangement.Drawing = new SalesArrangementParametersDrawing
        {
            RepaymentAccount = new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingRepaymentAccount
            {
                Prefix = "01",
                Number = "123456789",
                BankCode = "1111"
            },
            Applicant = new Identity(123, IdentitySchemes.Kb),
            DrawingDate = DateTime.Now,
            PayoutList =
            {
                new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList
                {
                    PrefixAccount = "01",
                    AccountNumber = "123456789",
                    DrawingAmount = 123456,
                    BankCode = "1111",
                    VariableSymbol = "V123456789",
                    ConstantSymbol = "C123456789",
                    SpecificSymbolUcetKeSplaceni = "S123456789"
                },
                new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList
                {
                    PrefixAccount = "02",
                    AccountNumber = "223456789",
                    DrawingAmount = 223456,
                    BankCode = "2222",
                    VariableSymbol = "V2123456789",
                    ConstantSymbol = "C2123456789",
                    SpecificSymbolUcetKeSplaceni = "S2123456789"
                }
            }
        };
    }
}