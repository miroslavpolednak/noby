from .modificators.ModificatorDate import ModificatorDate

class ModificationResolver():

    __MODIFICATORS = dict( 
        Date = ModificatorDate(),
    )

    def __init__(self, modification_value: str):
        
        # example: 'Date(days=40)'

        message_invalid = f'Invalid modification value [{modification_value}]'
        assert isinstance(modification_value, str), message_invalid

        index_bracket_open = modification_value.index('(')
        index_bracket_close = modification_value.index(')')
        assert index_bracket_open > 0 and index_bracket_close > 0 and index_bracket_close > index_bracket_open, message_invalid

        modificator_name = modification_value[0:index_bracket_open]
        params_expression =  modification_value[index_bracket_open+1:index_bracket_close]

        params_list = params_expression.split(',')
        params_dict = dict()
        for p in params_list:
            p_parts = p.split('=')
            assert len(p_parts) == 2, message_invalid

            p_name = p_parts[0]
            p_value = p_parts[1]
            params_dict[p_name] = p_value 

        self.__modificator_name = modificator_name
        self.__params_dict = params_dict

    @property
    def modificator_name(self):
        return self.__modificator_name

    @property
    def params_dict(self):
        return self.__params_dict

    def __str__ (self):
        return f'ModificationResolver [modificator_name: {self.__modificator_name} | params_dict: {self.__params_dict}]'

    def get_value(self):
        assert self.modificator_name in ModificationResolver.__MODIFICATORS.keys(), f'No modificator found [{self.modificator_name}]'
        modificator = ModificationResolver.__MODIFICATORS[self.modificator_name]
        return modificator.modify(**self.params_dict)
