from business.codebooks import EProductType, ELoanKind, EHouseholdType

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# DATA
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# EProductType:     Mortgage, MortgageBridging, MortgageWithoutIncome, MortgageNonPurposePart, MortgageAmerican
# ELoanKind:        Standard, MortgageWithoutRealty
# EHouseholdType:   Main, Codebtor, Garantor
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------


# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# OPTION RESOLVERS
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
def to_date(days: int = 0) -> str:
    from datetime import datetime, timedelta

    date: datetime = datetime.today() + timedelta(days=days)

    #return date.isoformat()
    return date.strftime('%Y-%m-%d')

def to_product_type_id(product_type) -> int:
    e = EProductType[product_type]
    return e.value

def to_loan_kind_id(loan_kind) -> int:
    e = ELoanKind[loan_kind]
    return e.value

def to_household_type_id(household_type) -> int:
    e = EHouseholdType[household_type]
    return e.value

def resolve_options(options: dict, option_resolvers: dict) -> dict:

    if (options is None):
        return None

    result = {}

    for k in options.keys():
        assert k in option_resolvers.keys(), f'Option resolver not specified [{k}]'
        target_key = option_resolvers[k]['target_key']
        fce = option_resolvers[k]['fce']
        value_in = options[k]
        value_out = fce(value_in)
        result[target_key] = value_out

    return result          

option_resolvers_household = {
    'HouseholdType': dict(fce = lambda value: to_household_type_id(value), target_key = 'householdTypeId'),
}

option_resolvers_offer = {
    'ExpectedDateOfDrawing': dict(fce = lambda value: to_date(value), target_key = 'expectedDateOfDrawing'),
    'ProductType': dict(fce = lambda value: to_product_type_id(value), target_key = 'productTypeId'),
    'LoanKind': dict(fce = lambda value: to_loan_kind_id(value), target_key = 'loanKindId'),
}

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------

class OptionsResolver():
        
    @staticmethod
    def resolve(options: dict, option_resolvers: dict) -> dict:
        
        if (options is None):
            return None

        result = {}

        for k in options.keys():
            assert k in option_resolvers.keys(), f'Option resolver not specified [{k}]'
            target_key = option_resolvers[k]['target_key']
            fce = option_resolvers[k]['fce']
            value_in = options[k]
            value_out = fce(value_in)
            result[target_key] = value_out

        return result          
        

