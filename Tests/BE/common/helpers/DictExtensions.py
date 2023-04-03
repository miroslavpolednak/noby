import jsonpath_ng
from typing import List, Any
from ..logging.Log import Log

class DictExtensions():

    __log: Log = None

    @staticmethod
    def __get_log() -> Log:
        if DictExtensions.__log is None:
            DictExtensions.__log = Log.getLogger('DictExtensions')
        return DictExtensions.__log

    @staticmethod
    def __get_json_paths(json: dict|list):
        if isinstance(json, dict):
            for key, value in json.items():
                yield f'.{key}'
                yield from (f'.{key}{p}' for p in DictExtensions.__get_json_paths(value))
        elif isinstance(json, list):
            for i, value in enumerate(json):
                yield f'[{i}]'
                yield from (f'[{i}]{p}' for p in DictExtensions.__get_json_paths(value))

    @staticmethod
    def get_json_paths(json: dict|list):
        paths: List[str] = [s[1:] if s.startswith('.') else s for s in DictExtensions.__get_json_paths(json)]
        return paths

    @staticmethod
    def modify_json(data_source: dict, data_modification: dict)->dict:
        assert isinstance(data_source, dict)
        assert isinstance(data_modification, dict)

        paths_source: List[str] = DictExtensions.get_json_paths(data_source)
        paths_modific: List[str] = DictExtensions.get_json_paths(data_modification)

        for p_mod in paths_modific:

            if p_mod not in paths_source:
                DictExtensions.__get_log().warn(f'Modification path [{p_mod}] not found in source json')
                continue

            value_source = DictExtensions.get_value_by_path(data_source, p_mod)

            if isinstance(value_source, dict | list):
                continue  

            value_modified = DictExtensions.get_value_by_path(data_modification, p_mod)
            DictExtensions.set_value_by_path(data_source, p_mod, value_modified)
            DictExtensions.__get_log().debug(f'Source json modified [{p_mod}: {value_source} -> {value_modified}]')
        
        return data_source

    @staticmethod
    def get_value_by_path(data: dict, path: str) -> Any:
        jsonpath_expr = jsonpath_ng.parse(path)
        matches = jsonpath_expr.find(data)
        matches_count = len(matches)
        assert matches_count == 1
        return matches[0].value
        
    @staticmethod
    def set_value_by_path(data: dict, path: str, value: Any):
        jsonpath_expr = jsonpath_ng.parse(path)
        matches = jsonpath_expr.find(data)
        matches_count = len(matches)
        assert matches_count == 1
        jsonpath_expr.update(data, value)


                # "childrenUpToTenYearsCount": 1,
                # "childrenOverTenYearsCount": 2,
                # "areCustomersPartners": true,
                # "expenses": {
                #     "savingExpenseAmount": 10000,
                #     "insuranceExpenseAmount": 3000,
                #     "housingExpenseAmount": 10000,
                #     "otherExpenseAmount": 7500
                # }