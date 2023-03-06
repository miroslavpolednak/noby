from typing import List

from common import Convertor

from .IToJsonValue import IToJsonValue

CHECK_ATTR_DISPATCH: bool = False

class Base(IToJsonValue):
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

            has_dispatch = k.lower() in self.__dispatches

            if CHECK_ATTR_DISPATCH:
                assert has_dispatch, f"Invalid attribute '{self.__class__.__name__}.{k}'"
            
            if not has_dispatch:
                print(f"Dispatch not found for attribute '{self.__class__.__name__}.{k}'!")
                continue

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

    def get_value(self, key: str) -> object:
        k_next = key.replace('_', '').lower()
        assert k_next in self.__dispatches, f"Invalid attribute '{key}'"
        dispatch = self.__dispatches[k_next]
        dispatch_key = dispatch['key']
        attr = f'_{self.__class__.__name__}__{dispatch_key}'
        return getattr(self, attr)


    def _get_json_keys(self) -> List[str]:
        raise NotImplementedError(f"Method '{self.class_name}._get_json_keys' not implemented!")


    def to_json_value(self) -> dict:

        keys: List[str] = self._get_json_keys()

        req = {}

        for k in keys:
            value = Convertor.to_json_value(self.get_value(k))
            req[k] = value

        return req

