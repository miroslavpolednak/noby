from datetime import datetime
from decimal import Decimal
from uuid import UUID

class Convertor():

    # @staticmethod
    # def to_val(value: object):
    #     return value

    @staticmethod
    def to_bool(value: object):

        if (value is None) or isinstance(value, bool):
            return value

        return bool(value)

    @staticmethod    
    def to_str(value: object):

        if (value is None) or isinstance(value, str):
            return value

        return str(value)

    @staticmethod    
    def to_guid(value: object):

        if (value is None) or isinstance(value, UUID):
            return value

        try:
            return UUID(str(value))
        except ValueError:
            raise ValueError(f'GUID conversion not supported [{type(value)}({value})]')

    @staticmethod    
    def to_int(value: object):

        if (value is None) or isinstance(value, int):
            return value

        if isinstance(value, str):
            return int(value)

        raise ValueError(f'INT conversion not supported [{type(value)}({value})]')

    @staticmethod
    def to_decimal(value: object):

        if (value is None) or isinstance(value, Decimal):
            return value

        try:
            return Decimal(str(value))
        except ValueError:
            raise ValueError(f'DECIMAL conversion not supported [{type(value)}({value})]')


    @staticmethod
    def to_date(value) -> datetime:

        if (value is None) or isinstance(value, datetime):
            return value

        if isinstance(value, str):
            return datetime.fromisoformat(value)
            
        raise ValueError(f'DATE conversion not supported [{type(value)}({value})]')


    @staticmethod
    def to_grpc(value: object):

        if (value is None) or isinstance(value, bool) or isinstance(value, int) or isinstance(value, str):
            return value

        if isinstance(value, UUID):
            return str(value)

        if isinstance(value, Decimal):
            nano_factor: Decimal = 1_000_000_000
            units = int(value)
            return dict (units = units, nanos = int((value - units) * nano_factor))

        if isinstance(value, datetime):
            return dict (
                year = value.year,
                month = value.month,
                day = value.day,
            )

        raise ValueError(f'GRPC conversion not supported [{type(value)}({value})]')

        # private const decimal NanoFactor = 1_000_000_000;

        # public NullableGrpcDecimal(long units, int nanos)
        # {
        #     Units = units;
        #     Nanos = nanos;
        # }

        # public static implicit operator double?(NullableGrpcDecimal? grpcDecimal)
        # {
        #     if (grpcDecimal == null) return default(double?);

        #     return Convert.ToDouble(grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor);
        # }

        # public static implicit operator decimal?(NullableGrpcDecimal? grpcDecimal)
        # {
        #     if (grpcDecimal == null) return default(decimal?);

        #     return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
        # }

        # public static implicit operator NullableGrpcDecimal?(decimal? value)
        # {
        #     if (!value.HasValue) return default(NullableGrpcDecimal);
            
        #     var units = decimal.ToInt64(value.Value);
        #     var nanos = decimal.ToInt32((value.Value - units) * NanoFactor);
        #     return new NullableGrpcDecimal(units, nanos);
        # }