import re
from typing import List
from common import EnumExtensions
from .EWorkflowType import EWorkflowType
from .EWorkflowEntity import EWorkflowEntity

class WorkflowStep():

    __WORKFLOW_TYPE_ITEMS: List[str] = EnumExtensions.enum_to_list(EWorkflowType, True)
    __WORKFLOW_ENTITY_ITEMS: List[EWorkflowEntity] = EnumExtensions.enum_to_list(EWorkflowEntity, False)

    def __init__(self, workflow_json: dict):

        path = workflow_json['path'] if 'path' in workflow_json.keys() else None
        type = workflow_json['type'] if 'type' in workflow_json.keys() else None
        data = workflow_json['data'] if 'data' in workflow_json.keys() else None
        
        assert path is not None, f"Invalid workflow definition - 'path' is required"
        assert type is not None, f"Invalid workflow definition - 'type' is required"

        assert isinstance(path, str), f"Invalid workflow definition - 'path' value is invalid [{path}]"

        if data is not None:
            assert isinstance(data, dict), f"Invalid workflow definition - 'data' value is invalid"

        workflow_type: EWorkflowType = EWorkflowType[type.upper()]

        if workflow_type == EWorkflowType.ADD or workflow_type == EWorkflowType.EDIT:
            assert type is not None, f"Invalid workflow definition - 'data' are required for given 'type' [{type}]"

        workflow_entity = self.__recognize_entity(path)
        assert workflow_entity is not None, f"Invalid workflow definition - 'path' is not supported ['{path}']"

        self.__path = path
        self.__entity = workflow_entity
        self.__type = workflow_type
        self.__data = data
        

    def __recognize_entity(self, path: str) -> EWorkflowEntity | None:
        entity: EWorkflowEntity = None

        for entity_item in WorkflowStep.__WORKFLOW_ENTITY_ITEMS:
            regex = entity_item.value
            if len(list(re.finditer(regex, path))) == 1:
                entity = entity_item
                break
        
        return entity

    @property
    def path(self) -> str:
        return self.__path

    @property
    def entity(self) -> EWorkflowEntity:
        return self.__entity

    @property
    def type(self) -> EWorkflowType:
        return self.__type

    @property
    def data(self) -> dict:
        return self.__data

    def __str__ (self):
        return f'WorkflowStep [path: {self.__path} | entity: {self.__entity} | type: {self.__type} | data: {self.__data}]'

        
        









        # {
        #     "path": "parameters.loanRealEstates",
        #     "type": "add",
        #     "data": {
        #         "realEstateTypeId": 5,
        #         "isCollateral": false,
        #         "realEstatePurchaseTypeId": 2
        #     }
        # }


        