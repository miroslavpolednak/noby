from .Base import Base

from common import Convertor

DISPATCHES = {
            'sum': lambda value: Convertor.to_decimal(value),
            'frequency': lambda value: Convertor.to_int(value),
        }

class Insurance(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Insurance(js_dict = js_dict)

    
# --------------------------------------------------------------------------------------------

# class Insurance():
#     def __init__(self, js_dict: dict = None):

#         self.__sum = None
#         self.__frequency = None

# --------------------------------------------------------------------------------------------