from .Base import Base

from common import Convertor

DISPATCHES = {
            'developer_id': lambda value: Convertor.to_int(value),
            'project_id': lambda value: Convertor.to_int(value),
            'new_developer_name': lambda value: Convertor.to_str(value),
            'new_developer_project_name': lambda value: Convertor.to_str(value),
            'new_developer_cin': lambda value: Convertor.to_str(value),
        }

class Developer(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Developer(js_dict = js_dict)


# --------------------------------------------------------------------------------------------

# class Developer():
#     def __init__(self, js_dict: dict = None):

#         self.__developer_id = None
#         self.__project_id = None
#         self.__new_developer_name = None
#         self.__new_developer_project_name = None
#         self.__new_developer_cin = None

# --------------------------------------------------------------------------------------------