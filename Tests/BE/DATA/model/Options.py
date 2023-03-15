from typing import List
from common import ETestEnvironment, ETestLayer, ETestType, EnumExtensions

class Options():
    def __init__(self, environments: ETestEnvironment, layers: ETestLayer,  types: ETestType):
        self.__environments = environments
        self.__layers = layers
        self.__types = types

    @property
    def environments(self) -> ETestEnvironment:
        return self.__environments

    @property
    def layers(self) -> ETestLayer:
        return self.__layers

    @property
    def types(self) -> ETestType:
        return self.__types

    def __str__ (self):
        environments = ','.join(EnumExtensions.enum_to_list(self.__environments, True))
        layers = ','.join(EnumExtensions.enum_to_list(self.__layers, True))
        types = ','.join(EnumExtensions.enum_to_list(self.__types, True))
        return f'Options [environments: {environments} | layers: {layers} | types: {types}]'

    @staticmethod
    def from_json(data: dict):

        list_environments: List[ETestEnvironment] = EnumExtensions.parse_items(ETestEnvironment, data['environments']) if 'environments' in data.keys() else EnumExtensions.enum_to_list(ETestEnvironment)
        list_layers: List[ETestLayer] = EnumExtensions.parse_items(ETestLayer, data['layers']) if 'layers' in data.keys() else EnumExtensions.enum_to_list(ETestLayer)
        list_types: List[ETestType] = EnumExtensions.parse_items(ETestType, data['types']) if 'types' in data.keys() else EnumExtensions.enum_to_list(ETestType)

        assert len(list_environments) > 0, "Option 'environments' does not contain any value !"
        assert len(list_layers) > 0, "Option 'layers' does not contain any value !"
        assert len(list_types) > 0, "Option 'types' does not contain any value !"

        environments: ETestEnvironment = EnumExtensions.items_to_flag(list_environments)
        layers: ETestLayer = EnumExtensions.items_to_flag(list_layers)
        types: ETestType = EnumExtensions.items_to_flag(list_types)

        return Options(environments, layers, types)