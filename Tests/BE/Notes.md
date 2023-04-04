# Notes

## Python

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


### Requirements
    pip freeze > requirements.txt
    pip install -r requirements.txt

### PyTest
    - python -m pytest
    - python -m pytest --collect-only
    - python -m pytest --no-header -v
    - python -m pytest --no-header -vv
    - python -m pytest --no-header -vv -s
    - python -m pytest --no-header -vv -s --log-cli-level=20
    - spouštět ze složky Test/BE (inicializační soubor '/Tests/BE/tests/__init__.py' zajistí načtení stubs)

https://www.programiz.com/python-programming/online-compiler/

### Modifications (regex)

https://medium.com/factory-mind/regex-tutorial-a-simple-cheatsheet-by-examples-649dc1c3f285

    - Date(days=10)
    - ProductType(type=Mortgage) [Mortgage, MortgageBridging, MortgageWithoutIncome, MortgageNonPurposePart, MortgageAmerican]
    - LoanKind(kind=Standard)    [Standard, MortgageWithoutRealty]
    - HouseholdType(type=Mai)    [Main, Codebtor, Garantor]
    