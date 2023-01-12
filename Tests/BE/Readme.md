# TESTS BE

## Environment

### VirutalEnv

        https://www.geeksforgeeks.org/creating-python-virtual-environment-windows-linux/

        VirtualEnv:
        - pip install virtualenv
        - virtualenv venv
        - venv\Scripts\activate

        Notes:
            virtualenv --version
            https:/go.microsoft.com/fwlink/?LinkID=135170 [Popisuje zásady spouštění PowerShellu a vysvětluje, jak je spravovat.]

            Je to problém při spouštění PowerShell scriptu z VS Code. Lze to povolit pomocí nastavení ExecutionPolicy -> Bypass:
            Get-ExecutionPolicy
            Set-ExecutionPolicy Bypass
            Set-ExecutionPolicy Restricted 


### GRPC
    pip install grpcio
    pip install grpcio-reflection
    pip install grpcio-tools


https://github.com/grpc/grpc/blob/master/doc/python/server_reflection.md

export PYTHONPATH='D:\Users\992466q\source\repos\OneSolutionTests'

ProtoTypes:
- bool
- cis.types.GrpcDate
- cis.types.GrpcDecimal
- cis.types.ModificationStamp
- cis.types.NullableGrpcDate
- cis.types.NullableGrpcDecimal
- google.protobuf.BoolValue
- google.protobuf.Int32Value
- google.protobuf.StringValue
- int32
- string
