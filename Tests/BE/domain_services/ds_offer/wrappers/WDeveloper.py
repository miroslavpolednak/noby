from .WBase import WBase

from Types_pb2 import Developer

class WDeveloper(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=Developer(), js_dict = js_dict)

# message Developer {
# 	google.protobuf.Int32Value DeveloperId = 1;
# 	google.protobuf.Int32Value ProjectId = 2;
# 	string NewDeveloperName = 3;
# 	string NewDeveloperProjectName = 4;
# 	string NewDeveloperCin = 5;
# }