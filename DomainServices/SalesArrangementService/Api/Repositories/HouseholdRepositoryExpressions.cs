﻿using System.Linq.Expressions;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal static class HouseholdRepositoryExpressions
{
    public static Expression<Func<Entities.Household, Contracts.Household>> HouseholdDetail()
    {
        return t => new Contracts.Household()
        {
            HouseholdId = t.HouseholdId,
            SalesArrangementId = t.SalesArrangementId,
            HouseholdTypeId = (int)t.HouseholdTypeId,
            CustomerOnSAId1 = t.CustomerOnSAId1,
            CustomerOnSAId2 = t.CustomerOnSAId2,
            Data = new HouseholdData
            {
                ChildrenOverTenYearsCount = t.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = t.ChildrenUpToTenYearsCount,
                PropertySettlementId = t.PropertySettlementId
            },
            Expenses = new Expenses
            {
                HousingExpenseAmount = t.HousingExpenseAmount,
                SavingExpenseAmount = t.SavingExpenseAmount,
                OtherExpenseAmount = t.OtherExpenseAmount,
                InsuranceExpenseAmount = t.InsuranceExpenseAmount
            }
        };
    }
}