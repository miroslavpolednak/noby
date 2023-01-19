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

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Developer)
        return value.to_grpc()

    def to_grpc(self) -> dict:

        # message Developer {
        #     google.protobuf.Int32Value DeveloperId = 1;
        #     google.protobuf.Int32Value ProjectId = 2;
        #     google.protobuf.StringValue NewDeveloperName = 3;
        #     google.protobuf.StringValue NewDeveloperProjectName = 4;
        #     google.protobuf.StringValue NewDeveloperCin = 5;
        # }

        # "Developer\": {\"DeveloperId\":1, \"ProjectId\":2, \"NewDeveloperCin\":\"cin\"}

        return dict(
            DeveloperId = Convertor.to_grpc(self.get_value('developer_id')),
            ProjectId = Convertor.to_grpc(self.get_value('project_id')),
            NewDeveloperName = Convertor.to_grpc(self.get_value('new_developer_name')),
            NewDeveloperProjectName = Convertor.to_grpc(self.get_value('new_developer_project_name')),
            NewDeveloperCin = Convertor.to_grpc(self.get_value('new_developer_cin')),
        )

# --------------------------------------------------------------------------------------------

# class Developer():
#     def __init__(self, js_dict: dict = None):

#         self.__developer_id = None
#         self.__project_id = None
#         self.__new_developer_name = None
#         self.__new_developer_project_name = None
#         self.__new_developer_cin = None

# --------------------------------------------------------------------------------------------