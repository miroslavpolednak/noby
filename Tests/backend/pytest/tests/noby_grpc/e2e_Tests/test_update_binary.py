import datetime

import datetime as datetime
import pytest
import requests
import pyodbc
from sqlalchemy.ext.declarative import declarative_base


#---------------------------------------------

import sys
from os.path import dirname
import importlib.util 

import grpc
import ssl

connection = pyodbc.connect(driver='{SQL Server Native Client 11.0}',
                                server='adpra173.vsstest.local\\FAT',
                                database='OfferService',
                                uid='testsql', pwd='Rud514')
connection.autocommit = True
cursor = connection.cursor()


cursor.execute('select * from [Offer] where OfferId=108')


for item in cursor:
    print (item.SimulationInputsBin)





#Import Stubs for CIS Types
sys.path.append(dirname(__file__) + '/../../../grpc/CisTypes')

#Import Stubs for Domain Services, fell free to append any needed
sys.path.append(dirname(__file__) + '/../../../grpc/OfferService')

#Import Stub for Service. This way is necessary because of dot in the name of file
spec = importlib.util.spec_from_file_location(
    name="OfferService",
    location="../../../grpc/OfferService/OfferService.v1_pb2_grpc.py",
)
OfferService = importlib.util.module_from_spec(spec)
spec.loader.exec_module(OfferService)
OfferServiceStub = OfferService.OfferServiceStub


import Mortgage.MortgageSimulationInputs_pb2 as MortgageSimulationInputs

msi=MortgageSimulationInputs.MortgageSimulationInputs()
msi.ParseFromString(item.SimulationInputsBin)

print('>>=======================================================================')
print(msi)
print('=========================================================================')
msi.LoanAmount.units+=1
print(msi)
print('<<=======================================================================')

upData=msi.SerializeToString()

cursor.execute('update [Offer] set SimulationInputsBin = ? where OfferId=108', upData)

cursor.close()
connection.close()
