from typing import List
from enum import Enum, Flag

class EnumExtensions():

    @staticmethod
    def enum_to_list(enum: Enum, as_str: bool = False) -> List[Enum] | List[str]:
        return list(e.name if as_str else e for e in enum)

    @staticmethod
    def parse_items(enum: Enum, items: List[str]) -> List[Enum]:

        if items is None:
            return None

        enum_items = list(item.name for item in enum)

        def str_to_enum(item: str) -> Enum:
            assert item in enum_items, f"Enum '{enum}' does not contain item '{item}' !"
            return enum[item]

        return list(map(str_to_enum, items))

    @staticmethod
    def items_to_flag(items: List[Flag]) -> Flag:
        assert len(items) > 0

        flag: Flag = None

        for i in items:
            flag = i if flag is None else flag | i
        
        return flag

    @staticmethod
    def flag_to_items(flag: Flag) -> List[Flag]:

        if flag is None:
            return None

        enum = type(flag)

        items = EnumExtensions.enum_to_list(enum, True)

        print(items)


