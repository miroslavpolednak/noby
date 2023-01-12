import os
from typing import List

from grpc_tools import protoc

ROOT_FOLDER = 'OneSolution'

# looks for foot folder by folder of this file
def find_root_folder():
    current_folder = os.path.dirname(__file__)
    assert ROOT_FOLDER in current_folder, f'Folder [{ROOT_FOLDER}] not found in current path [{current_folder}]!'
    i = current_folder.index(ROOT_FOLDER)
    return current_folder[0 : i + len(ROOT_FOLDER)]

root_folder_path = find_root_folder()
out_folder_path = os.path.dirname(__file__)
print(f'root_folder_path: {root_folder_path}')
print(f'out_folder_path: {out_folder_path}')

#print(os.path.join(root_folder_path,'CIS\\Infrastructure.gRPC.CisTypes\\Protos'))

# creates configuration for generating files (for one folder)
def get_conf(input: str, output: str) -> tuple[str, str]:
    i = os.path.join(root_folder_path, input)
    o = os.path.join(out_folder_path, output)
    return (i, o)

# creates or clears folder
def clear_folder(folder_path :str):

    # create if not exists
    if not os.path.exists(folder_path):
        os.makedirs(folder_path)
        return

    # clear existing folder
    for node in os.listdir(folder_path):
        node_path = os.path.join(folder_path, node)
        if os.path.isfile(node_path):
            #print('Deleting file:', node_path)
            os.remove(node_path)


# generates folder files
def generate_folder(conf: tuple[str, str], additional_inputs: List[str] = []):
    # print(f'generate_folder: {conf}')
    folder_in = conf[0]
    folder_out = conf[1]

    print(f'- generating GRPC folder > {os.path.basename(folder_out)}')

    # clear output folder
    clear_folder(folder_out)

    inputs = [*additional_inputs, folder_in]
    inputs_s = ' '.join(list(map(lambda i: f'-I{i}', inputs)))

    folder_in_list = []
    for dirpath, dirs, files in os.walk(os.path.dirname(folder_in)):
            proto_files = [ f for f in files if f.endswith( ('.proto') ) ]
            if (len(proto_files) > 0):
                folder_in_list.append(dirpath)
    
    #print(f'folder_in_list > {folder_in_list}')
            
    #for fi in list[folder_in_list.reverse]:
    for fi in folder_in_list:
        # build python command (grpc_tools.protoc)
        command = f'python -m grpc_tools.protoc {inputs_s} --python_out={folder_out} {fi}\\*proto'

        # run python command
        print(command)
        os.system(command)


def generate():

    protos_CIS: tuple[str, str] = get_conf('CIS\\Infrastructure.gRPC.CisTypes\\Protos\\', 'CisTypes')
    protos_DS: List[tuple[str, str]] = [
        get_conf('DomainServices\\CaseService\\Contracts\\', 'CaseService'),
        # get_conf('DomainServices\\CodebookService\\Contracts\\', 'CodebookService'),
        # get_conf('DomainServices\\DocumentArchiveService\\Contracts\\', 'DocumentArchiveService'),
        # get_conf('DomainServices\\HouseholdService\\Contracts\\', 'HouseholdService'),
        get_conf('DomainServices\\OfferService\\Contracts\\', 'OfferService'),
        # get_conf('DomainServices\\ProductService\\Contracts\\', 'ProductService'),
        # get_conf('DomainServices\\SalesArrangementService\\Contracts\\', 'SalesArrangementService'),
        # get_conf('DomainServices\\UserService\\Contracts\\', 'UserService'),
    ]

    # gen CIS
    generate_folder(protos_CIS)

    # gen DS
    for ds in protos_DS:
        generate_folder(ds, [protos_CIS[0]])


generate()


# # get current directory
# path = os.getcwd()
# print("Current Directory", path)
 
# # prints 4er45rt54r564er56453454r  directory
# print(os.path.abspath(os.path.join(path, os.pardir)))

# os.path.dirname(common)
# fl = os.path.abspath(os.path.dirname(__file__))
# d = os.pardir(os.path)



#PATH_TO_FOLDER_JSON: str = os.path.join(), 'json')








# par_in = 'D:\\Users\\992466q\\source\\repos\\OneSolution\\DomainServices\\CaseService\\Contracts\\'
# par_out = 'D:\\Users\\992466q\\source\\repos\\OneSolution\\Tests\\BE\\grpc\\CaseSvc'

# par_in = 'D:\\Users\\992466q\\source\\repos\\OneSolution\\CIS\\Infrastructure.gRPC.CisTypes\\Protos\\'
# par_out = 'D:\\Users\\992466q\\source\\repos\\OneSolution\\Tests\\BE\\grpc\\CisTypes_'
              
# #command = 'python -m grpc_tools.protoc -ID:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\   --python_out=D:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\temp D:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto'
# command = f'python -m grpc_tools.protoc -I{par_in} --python_out={par_out} {par_in}*proto'

# print(command)
# #command = f'python -m grpc_tools.protoc'
# os.system(command)




# python -m grpc_tools.protoc -I..\..\..\..\..\CIS\gRPC.CisTypes\Protos\ -I..\..\..\..\..\DomainServices\CaseService\Contracts\  --python_out=. --grpc_python_out=. ..\..\..\..\..\DomainServices\CaseService\Contracts\*proto

# protoc -I=$SRC_DIR --python_out=$DST_DIR $SRC_DIR/addressbook.proto

# --python -m grpc_tools.protoc -I../../protos --python_out=. --grpc_python_out=. ../../protos/helloworld.proto

# python -m grpc_tools.protoc -ID:\Users\992466q\source\repos\OneSolution\DomainServices\CaseService\Contracts\ --python_out=D:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\CaseSvc --grpc_python_out=D:\Users\992466q\source\repos\OneSolution\DomainServices\CaseService\Contracts\*proto

# D:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos

# python -m grpc_tools.protoc -I..\..\..\..\..\CIS\Infrastructure.gRPC.CisTypes\Protos\

# python -m grpc_tools.protoc -I..\..\..\CIS\Infrastructure.gRPC.CisTypes\Protos\   --python_out=. --grpc_python_out=. ..\..\..\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto

# D:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\temp
# Tests\BE\grpc\temp

# python -m grpc_tools.protoc -ID:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\   --python_out=. --grpc_python_out=. D:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto

# python -m grpc_tools.protoc -ID:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\   --python_out=D:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\temp D:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto
  


#python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\CisTypes d:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto

#python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService d:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\*proto

# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\OfferService.v1.proto --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService d:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\*proto

# python -m grpc_tools.protoc -I..\..\CIS\gRPC.CisTypes\Protos\ -I..\..\DomainServices\OfferService\Contracts\ -I..\..\DomainServices\OfferService\Contracts\Mortgage --python_out=. --grpc_python_out=. ..\..\DomainServices\OfferService\Contracts\*proto


# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService\*proto




# PS D:\Users\992466q\source\repos\OneSolution>  & 'D:\Users\992466q\AppData\Local\Programs\Python\Python311\python.exe' 'd:\Users\992466q\.vscode\extensions\ms-python.python-2022.20.1\pythonFiles\lib\python\debugpy\adapter/../..\debugpy\launcher' '53144' '--' 'd:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\gen.py'
# root_folder_path: d:\Users\992466q\source\repos\OneSolution
# out_folder_path: d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc
# - generating GRPC folder > CisTypes
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\CisTypes d:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto
# - generating GRPC folder > CaseService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\CaseService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\CaseService d:\Users\992466q\source\repos\OneSolution\DomainServices\CaseService\Contracts\*proto
# SearchCases.proto:7:1: warning: Import google/protobuf/wrappers.proto is unused.
# UpdateActiveTasks.proto:5:1: warning: Import GrpcDate.proto is unused.
# - generating GRPC folder > CodebookService
# - generating GRPC folder > DocumentArchiveService
# - generating GRPC folder > HouseholdService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\HouseholdService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\HouseholdService d:\Users\992466q\source\repos\OneSolution\DomainServices\HouseholdService\Contracts\CustomerOnSA\*proto
# CustomerOnSABase.proto: File not found.
# CustomerOnSA/CreateCustomer.proto:7:1: Import "CustomerOnSABase.proto" was not found or had errors.
# CustomerOnSA/CreateCustomer.proto:14:5: "CustomerOnSABase" is not defined.
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\HouseholdService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\HouseholdService d:\Users\992466q\source\repos\OneSolution\DomainServices\HouseholdService\Contracts\Household\*proto
# HouseholdExpenses.proto: File not found.
# HouseholdData.proto: File not found.
# Household/CreateHousehold.proto:6:1: Import "HouseholdExpenses.proto" was not found or had errors.
# Household/CreateHousehold.proto:7:1: Import "HouseholdData.proto" was not found or had errors.
# Household/CreateHousehold.proto:18:5: "DomainServices.HouseholdService.HouseholdData" is not defined.
# Household/CreateHousehold.proto:20:5: "DomainServices.HouseholdService.Expenses" is not defined.
# - generating GRPC folder > OfferService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService d:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\*proto
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService d:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\Mortgage\*proto
# - generating GRPC folder > ProductService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\ProductService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\ProductService d:\Users\992466q\source\repos\OneSolution\DomainServices\ProductService\Contracts\*proto
# - generating GRPC folder > SalesArrangementService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\SalesArrangementService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\SalesArrangementService d:\Users\992466q\source\repos\OneSolution\DomainServices\SalesArrangementService\Contracts\*proto
# CreateSalesArrangement.proto:7:1: warning: Import NullableGrpcDecimal.proto is unused.
# SalesArrangement.proto:5:1: warning: Import NullableGrpcDecimal.proto is unused.
# UpdateLoanAssessmentParameters.proto:6:1: warning: Import google/protobuf/wrappers.proto is unused.
# - generating GRPC folder > UserService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\UserService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\UserService d:\Users\992466q\source\repos\OneSolution\DomainServices\UserService\Contracts\*proto
# PS D:\Users\992466q\source\repos\OneSolution> 


# PS D:\Users\992466q\source\repos\OneSolution>  & 'D:\Users\992466q\AppData\Local\Programs\Python\Python311\python.exe' 'd:\Users\992466q\.vscode\extensions\ms-python.python-2022.20.1\pythonFiles\lib\python\debugpy\adapter/../..\debugpy\launcher' '53282' '--' 'd:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\gen.py'
# root_folder_path: d:\Users\992466q\source\repos\OneSolution
# out_folder_path: d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc
# - generating GRPC folder > CisTypes
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\CisTypes d:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\*proto
# - generating GRPC folder > OfferService
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService d:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\*proto
# python -m grpc_tools.protoc -Id:\Users\992466q\source\repos\OneSolution\CIS\Infrastructure.gRPC.CisTypes\Protos\ -Id:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\ --python_out=d:\Users\992466q\source\repos\OneSolution\Tests\BE\grpc\OfferService d:\Users\992466q\source\repos\OneSolution\DomainServices\OfferService\Contracts\Mortgage\*proto
# PS D:\Users\992466q\source\repos\OneSolution> 
