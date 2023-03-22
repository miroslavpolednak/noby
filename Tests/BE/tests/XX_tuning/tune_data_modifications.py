# ----------------------------
import _setup
# ----------------------------


# --------------------------------------------------------
# TEST DATA MODIFICATION
# --------------------------------------------------------
from typing import List
from DATA import JsonDataModificator

input_json_data: dict = dict(
    expectedDateOfDrawing = 'Date(days=-20)',
    householdTypeId = 'HouseholdType(type=Main)',
    productTypeId = 'ProductType(type=Mortgage)',
    loanKindId = 'LoanKind(kind=Standard)',

    loanPurposes = [
        dict(id = 202, sum = 3000000),
        dict(id = 203, sum = 3000000)
    ],
    
)

output_json_data: dict = JsonDataModificator.modify(input_json_data)

separator = '-'*100 
print(separator)
print('input_json_data', input_json_data)
print(separator)
print('output_json_data', output_json_data)
print(separator)

# --------------------------------------------------------