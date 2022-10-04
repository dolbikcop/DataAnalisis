import gspread as gsp
import numpy as np

gc = gsp.service_account(filename='unitydatascience-364214-ae1623ee9512.json')
sh = gc.open('UnitySheet')
price = np.random.randint(2000, 10000, 11)
mon = list(range(1, 11))
for i in range(1, len(mon)+1):
    tmpInf = str(((price[i]-price[i-1])/price[i-1])*100)
    tmpInf = tmpInf.replace('.', ',')
    sh.sheet1.update(('A'+ str(i+1)), str(i+1))
    sh.sheet1.update(('B'+ str(i+1)), str(price[i]))
    sh.sheet1.update(('C'+ str(i+1)), str(tmpInf))
    print(tmpInf)