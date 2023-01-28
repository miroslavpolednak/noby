from Pipeline_root import Pipeline_root
from Attribute_iterator import Attribute_iterator
from Attribute_array import Attribute_array
from Attribute_struct import Attribute_struct
from Writer import Writer

pipe=next=Pipeline_root()

name=next=Attribute_iterator("name",next,["Jakub","Kuba"])
name.register_eval(lambda doc, val: 'pan '+val)
surname=next=Attribute_iterator("surname",next,["Vanu","Vana"])
title=next=Attribute_iterator("title",next,["Mgr.", "Mr."])
full_name=next=Attribute_iterator("fullName",next)
full_name.register_eval(lambda doc, val:(doc['title']+" "+(doc['name'] if (doc['name']) else "")+" "+doc['surname']))
title.register_filter(lambda doc:(doc['__name']['count']+doc['__surname']['count']+doc['__title']['count']==2))

birth=next=Attribute_struct("birth",next)
birth_year=next=Attribute_iterator("birth.year",next,[1982,2002])
birth_month=next=Attribute_iterator("birth.month",next,[9,3])
birth_day=next=Attribute_iterator("birth.day",next,[5,13])
birth_year.register_filter(lambda doc:(doc['name']=='pan Kuba' and doc['birth']['year']==1982))
birth_year.register_filter(lambda doc:(doc['name']=='pan Jakub' and doc['birth']['year']==2002))
birth_day.register_filter(lambda doc:(doc['birth']['year']==2002 and doc['birth']['month']==3 and doc['birth']['day']==13))
birth_day.register_filter(lambda doc:(doc['birth']['year']==1982 and doc['birth']['month']==9 and doc['birth']['day']==5))

incomes=next=Attribute_array("incomes",next)
income0=next=Attribute_struct("incomes.[0]",next)
amount=next=Attribute_iterator("incomes.[0].amount",next,[100000,150000])
amount_yerly=next=Attribute_iterator("incomes.[0].amountYearly",next)
amount_yerly.register_eval(lambda doc, val:(12*doc['incomes'][0]['amount']))

writer=Writer(next)

pipe.process()

