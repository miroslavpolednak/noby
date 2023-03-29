from typing import List
from common import EnumExtensions
from .EWorkflowType import EWorkflowType

class WorkflowStep():

    __WORKFLOW_TYPE_ITEMS: List[str] = EnumExtensions.enum_to_list(EWorkflowType, True)

    def __init__(self, workflow_json: dict):

        path = workflow_json['path'] if 'path' in workflow_json.keys() else None
        type = workflow_json['type'] if 'type' in workflow_json.keys() else None
        data = workflow_json['data'] if 'data' in workflow_json.keys() else None
        
        assert path is not None, f"Invalid workflow definition - 'path' is required"
        assert type is not None, f"Invalid workflow definition - 'type' is required"

        assert isinstance(path, str), f"Invalid workflow definition - 'path' value is invalid"
        assert type.upper() in WorkflowStep.__WORKFLOW_TYPE_ITEMS, f"Invalid workflow definition - 'type' value is invalid"

        if data is not None:
            assert isinstance(data, dict), f"Invalid workflow definition - 'data' value is invalid"

        workflow_type: EWorkflowType = EWorkflowType[type.upper()]

        if workflow_type == EWorkflowType.ADD or workflow_type == EWorkflowType.EDIT:
            assert type is not None, f"Invalid workflow definition - 'data' are required for given 'type'"

        self.__path = path
        self.__type = workflow_type
        self.__data = data

    @property
    def path(self) -> str:
        return self.__path

    @property
    def type(self) -> EWorkflowType:
        return self.__type

    @property
    def data(self) -> dict:
        return self.__data

    def __str__ (self):
        return f'WorkflowStep [path: {self.__path} | type: {self.__type} | data: {self.__data}]'

        
        









        # {
        #     "path": "parameters.loanRealEstates",
        #     "type": "add",
        #     "data": {
        #         "realEstateTypeId": 5,
        #         "isCollateral": false,
        #         "realEstatePurchaseTypeId": 2
        #     }
        # }


        