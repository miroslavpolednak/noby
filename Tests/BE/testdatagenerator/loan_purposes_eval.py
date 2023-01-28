def create_loan_purposes(amount,split):
    lps=[]
    i=0
    sum=0
    for item in split:
        lps.append({})
        lps[i]["LoanPurposeId"]=item[0]
        lps[i]["Sum"]=amount*item[1]
        sum+=amount*item[1]
        i+=1
    #Prevent some rounding errors, user mistakes
    lps[i-1]["Sum"]=amount-(sum-lps[i-1]["Sum"])
    return lps

def loan_purposes_eval(doc,val):
    lps=0
    match val:
        case "AmVse":
            if(doc["ProductTypeId"]==20010):
                lps=create_loan_purposes(doc["LoanAmount"],[(201,0.2),(210,0.8)])
        case "Splaceni":
            if((doc["ProductTypeId"]==20010) or doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(201,1)])
        case "Koupe":
            if((doc["ProductTypeId"]==20001) or doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(202,1)])
        case "Vystavba":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(203,1)])
        case "Rekonstrukce":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(204,1)])
        case "ZhodnoceniPozemku":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(205,1)])
        case "Vyporadani":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(207,1)])
        case "PrevodDruztvo":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(208,1)])
        case "KoupeSpolecnosti":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(209,1)])
        case "Neucel":
            if((doc["ProductTypeId"]==20010) or doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(210,1)])
        case "2a":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(202,0.3),(203,0.7)]) #Koupě a výstavba
        case "2b":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(202,0.8),(204,0.2)]) #Koupě a rekonstrukce
        case "2c":
            if(doc["__"]["mainCase"]):
                lps=create_loan_purposes(doc["LoanAmount"],[(207,0.2),(209,0.8)]) #Vypořádání a koupě společnosti
    return lps 

