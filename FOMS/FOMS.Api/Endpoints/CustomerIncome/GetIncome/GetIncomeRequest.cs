﻿namespace FOMS.Api.Endpoints.CustomerIncome.GetIncome;

internal record GetIncomeRequest(int SalesArrangementId, int IncomeId)
    : IRequest<object>
{
}
