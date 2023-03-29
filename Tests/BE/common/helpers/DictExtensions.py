class DictExtensions():

    @staticmethod
    def get_json_paths(json: dict|list):
        if isinstance(json, dict):
            for key, value in json.items():
                yield f'.{key}'
                yield from (f'.{key}{p}' for p in DictExtensions.get_json_paths(value))
        elif isinstance(json, list):
            for i, value in enumerate(json):
                yield f'[{i}]'
                yield from (f'[{i}]{p}' for p in DictExtensions.get_json_paths(value))