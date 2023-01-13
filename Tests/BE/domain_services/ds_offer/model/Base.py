from typing import List

class Base():
    def __init__(self, dispatches: dict, js_dict: dict = None):

        assert dispatches is not None, f'Dispatches must be provided!'

        dispatches = dispatches.copy()
        #print(f'DISPATCHES > {dispatcshes}')

        for k in list(dispatches.keys()):
            k_next = k.replace('_', '').lower()
            dispatches[k_next] = dict(fce = dispatches[k], key = k)
            if k != k_next:
                del dispatches[k]

        self.__dispatches = dispatches

        self.__init_default()

        if js_dict is not None:
            self.__init_from_json(js_dict)

    def __init_default(self):
        for k in list(self.__dispatches.keys()):
            dispatch = self.__dispatches[k]
            dispatch_key = dispatch['key']
            val_default = None
            attr = f'_{self.__class__.__name__}__{dispatch_key}'
            setattr(self, attr, val_default)        

    def __init_from_json(self, js_dict: dict):
        assert js_dict is not None, f'Json data must be provided!'
        #print(js_dict)

        for k in list(js_dict.keys()):
            assert k.lower() in self.__dispatches, f"Invalid attribute '{k}'"

            dispatch = self.__dispatches[k.lower()]
            dispatch_fce = dispatch['fce']
            dispatch_key = dispatch['key']

            val_in = js_dict[k]
            val_out = dispatch_fce(val_in)

            attr = f'_{self.__class__.__name__}__{dispatch_key}'
            setattr(self, attr, val_out)        
        

    def __str__ (self):
        
        vals: List[str] = []

        for k in list(self.__dispatches.keys()):
            dispatch = self.__dispatches[k]
            dispatch_key = dispatch['key']
           
            attr = f'_{self.__class__.__name__}__{dispatch_key}'
            val = getattr(self, attr)

            if (val is not None):
                if isinstance(val, list):
                    val = ', '.join(list(map(lambda v: str(v), val)))
                    val = f'[{val}]'
                vals.append(f'{dispatch_key}: {val}')

        vals_s = ', '.join(vals)

        #return f'{self.class_name} [{list(self.__dispatches.keys())}]'
        #return f'{self.class_name} ({vals_s})'
        return f'({vals_s})'
        

    @property
    def class_name(self) -> object:
        return self.__class__.__name__