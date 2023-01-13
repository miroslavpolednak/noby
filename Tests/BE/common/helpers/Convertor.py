from datetime import datetime

class Convertor():

    @staticmethod
    def to_val(value: object):
        return value

    @staticmethod
    def to_bool(value: object):
        return value

    @staticmethod    
    def to_str(value: object):

        if (value is None):
            return None

        if isinstance(value, str):
            return value

        return str(value)


    @staticmethod    
    def to_int(value: object):

        if (value is None):
            return None

        if isinstance(value, int):
            return value

        if isinstance(value, str):
            return int(value)

        raise ValueError(f'Int conversion not supported [{value}]')

    @staticmethod
    def to_decimal(value: object):
        return value
        

    @staticmethod
    def to_date(value) -> datetime.date:

        if (value is None):
            return None

        if isinstance(value, type(datetime.date)):
            return value

        if isinstance(value, str):
            return datetime.fromisoformat(value).date()
            
        raise ValueError(f'Date conversion not supported [{value}]')
