# ----------------------------
import _setup
# ----------------------------

import jsonpath_ng

from typing import List

from business.offer import Offer
from business.case import Case

from tests.XX_tuning.data import load_json, save_json, clean_folder, EInputOutput


# CASE_PATHS: List[str] = [
#         'households[0].householdTypeId',
#         'households[0].childrenUpToTenYearsCount',

#         'households[0].customer1.firstName',
#         'households[0].customer1.lastName',
#         'households[0].customer1.dateOfBirth',
#         'households[0].customer1.phoneNumberForOffer',
#         'households[0].customer1.emailForOffer',
#         'households[0].customer1.roleId',
#         'households[0].customer1.identity.id',
#         'households[0].customer1.identity.scheme',
#     ]

def get_json_paths(json: dict):
    if isinstance(json, dict):
        for key, value in json.items():
            yield f'.{key}'
            yield from (f'.{key}{p}' for p in get_json_paths(value))
    elif isinstance(json, list):
        for i, value in enumerate(json):
            yield f'[{i}]'
            yield from (f'[{i}]{p}' for p in get_json_paths(value))


def assert_case(case_json_in: dict, case_json_out: dict):

    print('case_json_input:', case_json_in)
    print('case_json_out:', case_json_out)

    case_json_in_paths = [s[1:] for s in get_json_paths(case_json_in)]
    case_json_out_paths = [s[1:] for s in get_json_paths(case_json_out)]

    # concatenated list
    case_json_paths = [*case_json_in_paths, *case_json_out_paths]

    # list distinct
    list_set = set(case_json_paths)
    case_json_paths = (list(list_set))

    # list filter
    key_offer = 'offer.'
    key_offer_len = len(key_offer)

    # list final
    paths_to_assert = list(filter(lambda p: ('.' in p and p[0:key_offer_len] != key_offer) ,case_json_paths))
    paths_to_assert.sort()

    # for p in paths_to_assert:
    #     print(p)

    def get_value(path: str, json_data: dict, default = None) -> object:
        jsonpath_expr = jsonpath_ng.parse(path)
        matches = jsonpath_expr.find(json_data)
        matches_count = len(matches)
        assert matches_count <= 1, 'Found more matches ({matches_count}) for json path [{path}]'
        return matches[0].value if matches_count == 1 else default

    results_valid = []
    results_invalid = []
    for path in paths_to_assert:
        val_in = get_value(path, case_json_in)
        val_out = get_value(path, case_json_out)

        if isinstance(val_in, dict) or isinstance(val_in, list) or isinstance(val_out, dict) or isinstance(val_out, list):
            continue

        differs = val_in != val_out
        mark = '! ' if differs else ''
        result = f'{mark}path: {path} [{val_in} ---> {val_out}]' 
        
        results_list = results_invalid if differs else results_valid
        results_list.append(result)
        
        # assert val_in == val_out, path

    print('results_valid: ', len(results_valid))
    print('results_invalid: ', len(results_invalid))

    for r in results_invalid:
        print(r)


def run():
    case_input: dict = load_json('case.json', EInputOutput.Input)

    case_json_in: dict = Case.from_json(case_input).to_json_value()
    case_json_out: dict = load_json('case_3014487.json', EInputOutput.Output)

    assert_case(case_json_in, case_json_out)

#i = 'offer.fees'.index('offerx')

run()

# ForOffer ???
# ! path: households[0].areCustomersPartners [True ---> False]
# ! path: households[0].customer1.emailForOffer [novak@testcm.cz ---> None]
# ! path: households[0].customer1.identity.id [0 ---> None]
# ! path: households[0].customer1.identity.scheme [0 ---> None]
# ! path: households[0].customer1.phoneNumberForOffer [+420 777543234 ---> None]
# ! path: households[0].customer2.emailForOffer [novak@testcm.cz ---> None]
# ! path: households[0].customer2.identity.id [0 ---> None]
# ! path: households[0].customer2.identity.scheme [0 ---> None]
# ! path: households[0].customer2.phoneNumberForOffer [+420 777543234 ---> None]
# ! path: households[1].areCustomersPartners [None ---> False]
# ! path: households[1].childrenOverTenYearsCount [None ---> 0]
# ! path: households[1].childrenUpToTenYearsCount [None ---> 0]
# ! path: households[1].customer1.firstName [None ---> ]
# ! path: households[1].customer1.lastName [None ---> ]
# ! path: households[1].customer1.roleId [None ---> 128]
# ! path: households[2].areCustomersPartners [None ---> False]
# ! path: households[2].childrenOverTenYearsCount [None ---> 0]
# ! path: households[2].childrenUpToTenYearsCount [None ---> 0]
# ! path: households[2].customer1.firstName [None ---> ]
# ! path: households[2].customer1.lastName [None ---> ]
# ! path: households[2].customer1.roleId [None ---> 128]
# ! path: parameters.expectedDateOfDrawing [2023-04-01 ---> None]

# ! path: households[0].customer1.incomes[0].data.employer.birthNumber [None ---> ]
# ! path: households[0].customer1.incomes[1].data.cin [None ---> ]
# ! path: households[0].customer1.incomes[1].data.hasProofOfIncome [False ---> None]
# ! path: households[0].customer1.incomes[1].data.hasWageDeduction [False ---> None]
# ! path: households[0].customer1.incomes[1].data.incomeConfirmation.isIssuedByExternalAccountant [False ---> None]
# ! path: households[0].customer1.incomes[1].data.job.isInProbationaryPeriod [False ---> None]
# ! path: households[0].customer1.incomes[1].data.job.isInTrialPeriod [False ---> None]
# ! path: households[0].customer1.incomes[2].data.hasProofOfIncome [False ---> None]
# ! path: households[0].customer1.incomes[2].data.hasWageDeduction [False ---> None]
# ! path: households[0].customer1.incomes[2].data.incomeConfirmation.isIssuedByExternalAccountant [False ---> None]
# ! path: households[0].customer1.incomes[2].data.job.isInProbationaryPeriod [False ---> None]
# ! path: households[0].customer1.incomes[2].data.job.isInTrialPeriod [False ---> None]
# ! path: households[0].customer1.incomes[3].data.hasProofOfIncome [False ---> None]
# ! path: households[0].customer1.incomes[3].data.hasWageDeduction [False ---> None]
# ! path: households[0].customer1.incomes[3].data.incomeConfirmation.isIssuedByExternalAccountant [False ---> None]
# ! path: households[0].customer1.incomes[3].data.job.isInProbationaryPeriod [False ---> None]
# ! path: households[0].customer1.incomes[3].data.job.isInTrialPeriod [False ---> None]