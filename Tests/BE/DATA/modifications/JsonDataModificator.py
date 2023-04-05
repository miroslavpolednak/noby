import jsonpath_ng, re

from typing import List
from common import Log, DictExtensions

from .modificators._Modificator import Modificator
from .modificators.ModificatorDate import ModificatorDate
from .modificators.ModificatorProductType import ModificatorProductType
from .modificators.ModificatorLoanKind import ModificatorLoanKind
from .modificators.ModificatorHouseholdType import ModificatorHouseholdType
class JsonDataModificator():

    __log = Log.getLogger('JsonDataModificator')

    # all available(implemented) modificators
    __MODIFICATORS: List[Modificator] = [
            ModificatorDate(),
            ModificatorProductType(),
            ModificatorLoanKind(),
            ModificatorHouseholdType(),
        ]

    # searchs available modificators by regex 
    @staticmethod
    def find_modificator(expression: str) -> Modificator:

        if not isinstance(expression, str):
            return None

        modificator: Modificator = None

        for m in JsonDataModificator.__MODIFICATORS:
            if re.search(m.regex, expression) is not None:
                modificator = m
                break

        return modificator


    @staticmethod
    def modify(input_json_data: dict) -> dict:

        assert isinstance(input_json_data, dict), "Parameter 'input_json_data' must be DICT"

        json_paths = DictExtensions.get_json_paths(input_json_data)

        output_json_data = input_json_data.copy()

        for path in json_paths:
            jsonpath_expr = jsonpath_ng.parse(path)
            matches = jsonpath_expr.find(output_json_data)
            matches_count = len(matches)
            assert matches_count == 1
            value_current = matches[0].value
            modificator = JsonDataModificator.find_modificator(value_current)
            if modificator is None:
                continue

            value_modified = modificator.get_value(value_current)
            #print(f'{path}: {value_current} -> {value_modified}')
            jsonpath_expr.update(output_json_data, value_modified)
            JsonDataModificator.__log.info(f'Json Data Modification [{path}: {value_current} -> {value_modified}]')

        return output_json_data