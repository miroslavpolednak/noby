# ----------------------------
import _setup
# ----------------------------

# https://fat.noby.cz/undefined#/

from typing import List

from tests.tuning.OptionsResolver import OptionsResolver
from tests.tuning.data import load_json, save_json, clean_folder, EInputOutput

from E2E import ApiProcessor, ApiReaderOffer, ApiReaderCase

offer_case_json: dict = load_json('case.json', EInputOutput.Input)

results = ApiProcessor.process_list([offer_case_json])
for result in results:
    print('Result: ', result)

    if 'case_id' in result.keys():
        case_id = result['case_id']
        case = ApiReaderCase().load(case_id)
        case_json = case.to_json_value()
        save_json(f'case_{case_id}.json', EInputOutput.Output, case_json)
        print(f'https://fat.noby.cz/undefined#/mortgage/case-detail/{case_id}')
