gRPC Caller
Grpcurl wrapper for NOBY project
Jakub Váňa (jakub.vana@mpss.cz, jakub_vana@kb.cz)

Install
-------
For use of this tool you need to have installed grpcurt first. 
See docs: https://docs.microsoft.com/aspnet/core/grpc/test-tools?view=aspnetcore-6.0

./install.ps1  #This will install (or overwrite) configuration file in ~\.gc\ directory. Config file is necessary.
cp gc.ps1 yourBinDirectory #Copy gc.ps1 into directory where $PATH environment variable is set to.


Usage
-----
gc ENV[.NODE] Service.Method RequestData.json [MergeField=Value ...]

ENV: NOBY environment (e.g. DEV, FAT, SIT1, UAT)
NODE: Server in load balanced environments e.g. A for adpra191 of FAT environment od B for adrpra192
Service: Short name for service - as configured in config file e.g. Case
Method: Method to Call
RequestData.json: File with prepared request data to be send in .json format. Mergefields are supported - more info in Json section
MergeField: Name of mergefield to be replaced by Value in RequestData.json before sent to Server
Value: New value for mergefield

Example:
gc.ps1 FAT.B Customer.SearchCustomer SearchCustomer.json first=Jakub last=Novák

Json
----
There is support for mergefields in json with some limitations.

1) Because of basic regexp usage there is support just for one mergefield per line. 
2) Usage of some special characters can do some problems. Please report if any.

Mergefield definition is ${Label} or ${Label|defaultValue}
If default value is not provided, value must be set on command line.

Some examples

SearchCustomer.json:
{
    "NaturalPerson":
    {
        "FirstName":"${first|jan}",
        "LastName":"${last}"
    }
}

Input data for gc.ps1 FAT.B Customer.SearchCustomer SearchCustomer.json first=Jakub last=Novák:
{
    "NaturalPerson":
    {
        "FirstName":"Jakub",
        "LastName":"Novák"
    }
}

Input data for gc.ps1 FAT.B Customer.SearchCustomer SearchCustomer.json last=Novák:
{
    "NaturalPerson":
    {
        "FirstName":"jan",
        "LastName":"Novák"
    }
}

Input data for gc.ps1 FAT.B Customer.SearchCustomer SearchCustomer.json first=Jakub:
{
    "NaturalPerson":
    {
        "FirstName":"Jakub",
        "LastName":"${last}"                   <-- Invalid value, must be set on command line
    }
}

