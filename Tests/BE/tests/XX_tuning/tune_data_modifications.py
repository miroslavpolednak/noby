# ----------------------------
import _setup
# ----------------------------


# --------------------------------------------------------
# TEST DATA MODIFICATOR
# --------------------------------------------------------
import json
from typing import List
from DATA import JsonDataModificator, ModificationResolver

modifications: List[dict] = [
    dict(path = 'expectedDateOfDrawing', value = 'Date(days=40)')
]

input_json_data: dict = dict(
    productTypeId = 20001,
    loanPurposes = [
        dict(id = 202, sum = 3000000),
        dict(id = 203, sum = 3000000)
    ],
    expectedDateOfDrawing = "2023-04-01T00:00:00.0000"
)

resolver = ModificationResolver(modifications[0]['value'])
value = resolver.get_value()
print('resolver', resolver)
print('value', value)

output_json_data: dict = JsonDataModificator.modify(input_json_data, modifications)

separator = '-'*100 

print(separator)
print('input_json_data', input_json_data)
print(separator)
print('modifications', modifications)
print(separator)
print('output_json_data', output_json_data)
print(separator)

# --------------------------------------------------------



# # --------------------------------------------------------------------------------------
# # TEST
# # --------------------------------------------------------------------------------------
# modifications: List[dict] = [
#     dict(path = 'expectedDateOfDrawing', value = 'Date[days=40]')
# ]

# input_json_data: dict = dict(
#     productTypeId = 20001,
#     loanPurposes = [
#         dict(id = 202, sum = 3000000),
#         dict(id = 203, sum = 3000000)
#     ],
#     expectedDateOfDrawing = "2023-04-01T00:00:00.0000"
# )
# output_json_data: dict = JsonDataModificator.modify(input_json_data, modifications)

# separator = '-'*100 

# print(separator)
# print('input_json_data', input_json_data)
# print(separator)
# print('modifications', modifications)
# print(separator)
# print('output_json_data', output_json_data)
# print(separator)
# # --------------------------------------------------------------------------------------
