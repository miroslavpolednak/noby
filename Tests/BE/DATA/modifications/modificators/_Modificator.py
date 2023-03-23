from enum import Enum

from common import EnumExtensions

class Modificator:
    
    # https://medium.com/factory-mind/regex-tutorial-a-simple-cheatsheet-by-examples-649dc1c3f285

    def __init__(self, regex: str):
        self.__regex = regex

    @property
    def regex(self) -> str:
        """Returns reqular expression """
        return self.__regex

    def modify(self):
        pass

    def check_enum_name(self, enum: Enum, name: str):
        enum_names = EnumExtensions.enum_to_list(enum, True)
        enum_names_text = ', '.join(enum_names)
        assert name in enum_names, f"Enum item '{name}' not found in enum '{enum}'. Valid items: {enum_names_text}."

    def get_value(self, expression: str):
        
        # example: 'Date(days=40)'

        message_invalid = f'Invalid modification value [{expression}]'
        assert isinstance(expression, str), message_invalid

        index_bracket_open = expression.index('(')
        index_bracket_close = expression.index(')')
        assert index_bracket_open > 0 and index_bracket_close > 0 and index_bracket_close > index_bracket_open, message_invalid

        
        params_expression =  expression[index_bracket_open+1:index_bracket_close]

        params_list = params_expression.split(',')
        params_dict = dict()
        for p in params_list:
            p_parts = p.split('=')
            assert len(p_parts) == 2, message_invalid

            p_name = p_parts[0]
            p_value = p_parts[1]
            params_dict[p_name] = p_value 

        return self.modify(**params_dict)