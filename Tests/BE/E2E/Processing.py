# --------------------------------------------------------------------------------------------------------------
# Processing data helpers
# --------------------------------------------------------------------------------------------------------------

from .EProcessKey import EProcessKey

class Processing():

    __KEY_PROCESS_DATA = 'PROCESS_DATA'

    @staticmethod
    def init_key(entity_json: dict, key: str, value = dict()):
        if entity_json is None:
            return entity_json

        if key not in entity_json:
            entity_json[key] = value

        if entity_json[key] is None:
            entity_json[key] = value

        return entity_json[key]

    @staticmethod
    def get_key(entity_json: dict, key: str, default = None):
        if entity_json is None:
            return None

        if key not in entity_json:
            return default

        if entity_json[key] is None:
            return default

        return entity_json[key]

    @staticmethod
    def set_process_key(entity_json: dict, key: EProcessKey, value: object) -> dict:
        if entity_json is None:
            return entity_json

        if Processing.__KEY_PROCESS_DATA not in entity_json:
            entity_json[Processing.__KEY_PROCESS_DATA] = dict()
        
        entity_json[Processing.__KEY_PROCESS_DATA][key.value] = value

        return entity_json

    @staticmethod
    def get_process_key(entity_json: dict, key: EProcessKey) -> object:
        if entity_json is None:
            return entity_json

        if Processing.__KEY_PROCESS_DATA not in entity_json:
            return None

        if key.value not in entity_json[Processing.__KEY_PROCESS_DATA]:
            return None
        
        return entity_json[Processing.__KEY_PROCESS_DATA][key.value]