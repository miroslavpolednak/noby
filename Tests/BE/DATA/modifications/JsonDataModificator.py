from typing import List

from .ModificationResolver import ModificationResolver

class JsonDataModificator():
        
    @staticmethod
    def modify(input_json_data: dict, modifications: List[dict]) -> dict:
        
        assert isinstance(input_json_data, dict)
        
        if (len(modifications) == 0):
            return input_json_data

        output_json_data = input_json_data.copy()
       
        return output_json_data