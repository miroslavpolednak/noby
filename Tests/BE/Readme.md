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

- https://github.com/grpc/grpc/blob/master/doc/python/server_reflection.md

export PYTHONPATH='D:\Users\992466q\source\repos\OneSolutionTests'

#### ProtoTypes:
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

# Azure DevOps

## New AgentPool

    - https://learn.microsoft.com/en-us/azure/devops/pipelines/agents/v2-windows?view=azure-devops#permissions
    - https://azureops.org/articles/azure-devops-self-hosted-agent/

### Agent Inputs:

----------------------------------------------------------------------------------------------------------------------------------------
Template:
- Enter server URL > https://dev.azure.com/<organisation name>/
- Enter authentication type (press enter for PAT) > <Press enter>
- Enter personal access token > <Enter access token generated in step 1> If the connection is successful, it will ask you to register an agent.
- Enter agent pool (press enter for default)> <Name of the agent pool created earlier (OnPremAgentPool)>
- Enter agent name (press enter for <servername>) > <Provide any suitable name for the agent. The default is Server name>
- It will connect and test the agent connection. If it is successful,
- Enter work folder (press enter for _work) > <press enter>
- Enter run agent as service? (Y/N) (press enter for N) > (Enter Y if you want to run this as a service)
- Enter configure autologon and run agent on startup? (Y/N) (press enter for N) > (Enter Y if you want to start the service on startup)

----------------------------------------------------------------------------------------------------------------------------------------
Real:
- Enter server URL > https://tfs.kb.cz/tfs/MPSS/    https://tfs.kb.cz/tfs/  https://tfs.kb.cz/tfs/MPSS/CIS/
- Enter authentication type (press enter for PAT) > 
- Enter personal access token > <Enter access token generated in step 1> If the connection is successful, it will ask you to register an agent.
- Enter agent pool (press enter for default)> <Name of the agent pool created earlier (OnPremAgentPool)>
- Enter agent name (press enter for <servername>) > <Provide any suitable name for the agent. The default is Server name>
- It will connect and test the agent connection. If it is successful,
- Enter work folder (press enter for _work) > <press enter>
- Enter run agent as service? (Y/N) (press enter for N) > (Enter Y if you want to run this as a service)
- Enter configure autologon and run agent on startup? (Y/N) (press enter for N) > (Enter Y if you want to start the service on startup)
----------------------------------------------------------------------------------------------------------------------------------------


## Python
    - compiler: https://www.programiz.com/python-programming/online-compiler/
    - logging: https://realpython.com/python-logging/
