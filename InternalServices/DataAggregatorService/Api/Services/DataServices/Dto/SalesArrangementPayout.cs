using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.Dto;

internal class SalesArrangementPayout
{
    private readonly SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList _item;

    public SalesArrangementPayout(SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList item)
    {
        _item = item;
    }

    public GrpcDecimal DrawingAmount => _item.DrawingAmount;

    public string BankAccount => $"{_item.PrefixAccount}-{_item.AccountNumber}";

    public string BankCode => _item.BankCode;

    public string VariableSymbol => _item.VariableSymbol;

    public string ConstantSymbol => _item.ConstantSymbol;

    public string SpecificSymbol => _item.SpecificSymbolUcetKeSplaceni;
}