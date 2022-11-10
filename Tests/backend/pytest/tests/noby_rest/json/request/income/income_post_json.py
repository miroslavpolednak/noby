json_req_income_employer_rc_without_proof = {
    "sum": 200000,
    "currencyCode": "CZK",
    "incomeTypeId": 1,
    "data": {
        "employer": {
            "name": "Marvel",
            "cin": "2891",
            "birthNumber": None,
            "countryId": 16
        },
        "job": {
            "jobDescription": None,
            "firstWorkContractSince": None,
            "employmentTypeId": None,
            "currentWorkContractSince": None,
            "currentWorkContractTo": None,
            "grossAnnualIncome": None,
            "isInTrialPeriod": False,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": None,
            "deductionPayments": None,
            "deductionOther": None
        },
        "incomeConfirmation": {
            "confirmationDate": None,
            "confirmationPerson": None,
            "confirmationContact": None,
            "isIssuedByExternalAccountant": False
        },
        "hasProofOfIncome": False,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": False,
        "cin": None,
        "birthNumber": None,
        "countryOfResidenceId": None,
        "incomeOtherTypeId": None
    }
}

json_req_income_employer_cin_without_proof = {
    "sum": 40000,
    "currencyCode": "CZK",
    "incomeTypeId": 1,
    "data": {
        "employer": {
            "name": "DC",
            "cin": None,
            "birthNumber": "9854894",
            "countryId": 16
        },
        "job": {
            "jobDescription": None,
            "firstWorkContractSince": None,
            "employmentTypeId": None,
            "currentWorkContractSince": None,
            "currentWorkContractTo": None,
            "grossAnnualIncome": None,
            "isInTrialPeriod": False,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": None,
            "deductionPayments": None,
            "deductionOther": None
        },
        "incomeConfirmation": {
            "confirmationDate": None,
            "confirmationPerson": None,
            "confirmationContact": None,
            "isIssuedByExternalAccountant": False
        },
        "hasProofOfIncome": False,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": False,
        "cin": None,
        "birthNumber": None,
        "countryOfResidenceId": None,
        "incomeOtherTypeId": None
    }
}

# trialPeriod, cin employer, hasWageDeduction, ExternalAccountant
json_req_income_employer_cin_with_proof = {
    "sum": 78000,
    "currencyCode": "CZK",
    "incomeTypeId": 1,
    "data": {
        "employer": {
            "name": "Marvel",
            "cin": "7894216",
            "birthNumber": None,
            "countryId": 16
        },
        "job": {
            "jobDescription": "Ironman",
            "firstWorkContractSince": "2000-01-01T00:00:00.000Z",
            "employmentTypeId": 1,
            "currentWorkContractSince": "2020-01-01T00:00:00.000Z",
            "currentWorkContractTo": "2040-05-29T00:00:00.000Z",
            "grossAnnualIncome": 1500000,
            "isInTrialPeriod": True,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": 123,
            "deductionPayments": 456,
            "deductionOther": 789
        },
        "incomeConfirmation": {
            "confirmationDate": "2022-11-01T00:00:00.000Z",
            "confirmationPerson": "Pepper Watson",
            "confirmationContact": "777444111",
            "isIssuedByExternalAccountant": True
        },
        "hasProofOfIncome": True,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": True,
        "cin": None,
        "birthNumber": None,
        "countryOfResidenceId": None,
        "incomeOtherTypeId": None
    }
}

# probationaryPeriod, rc employer
json_req_income_employer_rc_with_proof = {
  "sum": 78000,
  "currencyCode": "CZK",
  "incomeTypeId": 1,
  "data": {
    "employer": {
      "name": "DC",
      "cin": "",
      "birthNumber": "8911074455",
      "countryId": 16
    },
    "job": {
      "jobDescription": "Ironman",
      "firstWorkContractSince": "2000-01-01T00:00:00.000Z",
      "employmentTypeId": 1,
      "currentWorkContractSince": "2020-01-01T00:00:00.000Z",
      "currentWorkContractTo": "2040-05-29T00:00:00.000Z",
      "grossAnnualIncome": 1500000,
      "isInTrialPeriod": False,
      "isInProbationaryPeriod": True
    },
    "wageDeduction": {
      "deductionDecision": None,
      "deductionPayments": None,
      "deductionOther": None
    },
    "incomeConfirmation": {
      "confirmationDate": "2022-11-01T00:00:00.000Z",
      "confirmationPerson": "Pepper Watson",
      "confirmationContact": "777444111",
      "isIssuedByExternalAccountant": False
    },
    "hasProofOfIncome": True,
    "foreignIncomeTypeId": None,
    "hasWageDeduction": False,
    "cin": None,
    "birthNumber": None,
    "countryOfResidenceId": None,
    "incomeOtherTypeId": None
  }
}

json_req_income_business_rc = {
    "sum": 5000000,
    "currencyCode": "CZK",
    "incomeTypeId": 2,
    "data": {
        "employer": {
            "name": None,
            "cin": None,
            "birthNumber": None,
            "countryId": None
        },
        "job": {
            "jobDescription": None,
            "firstWorkContractSince": None,
            "employmentTypeId": None,
            "currentWorkContractSince": None,
            "currentWorkContractTo": None,
            "grossAnnualIncome": None,
            "isInTrialPeriod": False,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": None,
            "deductionPayments": None,
            "deductionOther": None
        },
        "incomeConfirmation": {
            "confirmationDate": None,
            "confirmationPerson": None,
            "confirmationContact": None,
            "isIssuedByExternalAccountant": False
        },
        "hasProofOfIncome": False,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": False,
        "cin": None,
        "birthNumber": "89111075555",
        "countryOfResidenceId": 16,
        "incomeOtherTypeId": None
    }
}

json_req_income_business_cin = {
    "sum": 4000000,
    "currencyCode": "CZK",
    "incomeTypeId": 2,
    "data": {
        "employer": {
            "name": None,
            "cin": None,
            "birthNumber": None,
            "countryId": None
        },
        "job": {
            "jobDescription": None,
            "firstWorkContractSince": None,
            "employmentTypeId": None,
            "currentWorkContractSince": None,
            "currentWorkContractTo": None,
            "grossAnnualIncome": None,
            "isInTrialPeriod": False,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": None,
            "deductionPayments": None,
            "deductionOther": None
        },
        "incomeConfirmation": {
            "confirmationDate": None,
            "confirmationPerson": None,
            "confirmationContact": None,
            "isIssuedByExternalAccountant": False
        },
        "hasProofOfIncome": False,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": False,
        "cin": "8807052211",
        "birthNumber": None,
        "countryOfResidenceId": 16,
        "incomeOtherTypeId": None
    }
}

json_req_income_rent = {
    "sum": 29000,
    "currencyCode": "CZK",
    "incomeTypeId": 3,
    "data": {
        "employer": {
            "name": None,
            "cin": None,
            "birthNumber": None,
            "countryId": None
        },
        "job": {
            "jobDescription": None,
            "firstWorkContractSince": None,
            "employmentTypeId": None,
            "currentWorkContractSince": None,
            "currentWorkContractTo": None,
            "grossAnnualIncome": None,
            "isInTrialPeriod": False,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": None,
            "deductionPayments": None,
            "deductionOther": None
        },
        "incomeConfirmation": {
            "confirmationDate": None,
            "confirmationPerson": None,
            "confirmationContact": None,
            "isIssuedByExternalAccountant": False
        },
        "hasProofOfIncome": False,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": False,
        "cin": None,
        "birthNumber": None,
        "countryOfResidenceId": None,
        "incomeOtherTypeId": None
    }
}

json_req_income_other = {
    "sum": 150000,
    "currencyCode": "CZK",
    "incomeTypeId": 4,
    "data": {
        "employer": {
            "name": None,
            "cin": None,
            "birthNumber": None,
            "countryId": None
        },
        "job": {
            "jobDescription": None,
            "firstWorkContractSince": None,
            "employmentTypeId": None,
            "currentWorkContractSince": None,
            "currentWorkContractTo": None,
            "grossAnnualIncome": None,
            "isInTrialPeriod": False,
            "isInProbationaryPeriod": False
        },
        "wageDeduction": {
            "deductionDecision": None,
            "deductionPayments": None,
            "deductionOther": None
        },
        "incomeConfirmation": {
            "confirmationDate": None,
            "confirmationPerson": None,
            "confirmationContact": None,
            "isIssuedByExternalAccountant": False
        },
        "hasProofOfIncome": False,
        "foreignIncomeTypeId": None,
        "hasWageDeduction": False,
        "cin": None,
        "birthNumber": None,
        "countryOfResidenceId": None,
        "incomeOtherTypeId": 21
    }
}
