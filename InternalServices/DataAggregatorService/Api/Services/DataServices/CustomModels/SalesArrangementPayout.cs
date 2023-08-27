﻿using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class SalesArrangementPayout
{
    private readonly SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList _item;

    public SalesArrangementPayout(SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList item)
    {
        _item = item;
    }

    public GrpcDecimal DrawingAmount => _item.DrawingAmount;

    public string BankAccount => string.Join('-', new[] { _item.PrefixAccount, _item.AccountNumber }.Where(str => !string.IsNullOrWhiteSpace(str)));

    public string BankCode => _item.BankCode;

    public string VariableSymbol => _item.VariableSymbol;

    public string ConstantSymbol => _item.ConstantSymbol;

    public string SpecificSymbol => _item.SpecificSymbolUcetKeSplaceni;
}