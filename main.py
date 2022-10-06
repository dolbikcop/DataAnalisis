import gspread as gsp
import numpy as np

def model(a, b, x):
    return a * x + b


def loss_function(a, b, x, y):
    num = len(x)
    prediction = model(a, b, x)
    return (0.5 / num) * (np.square(prediction - y)).sum()

def optimize(a, b, x, y):
    num = len(x)
    prediction = model(a, b, x)
    da = (1.0 / num) * ((prediction - y) * x).sum()
    db = (1.0 / num) * ((prediction - y).sum())
    a = a - Lr * da
    b = b - Lr * db
    return a, b

def iterate(a, b, x, y, times):
    for i in range(times):
        a, b = optimize(a, b, x, y)
    return a, b

gc = gsp.service_account(filename='unitydatascience-364214-ae1623ee9512.json')
sh = gc.open('UnitySheet')
Lr = 0.000001

x = [3, 21, 22, 34, 54, 34, 55, 67, 89, 99]
x = np.array(x)
y = [2, 22, 24, 65, 79, 82, 55, 130, 150, 199]
y = np.array(y)

times = [1, 2, 3, 4, 5, 10, 100, 1000, 10000]

for i in range(1, len(times)+1):
    a, b = np.random.rand(1), np.random.rand(1)
    a, b = iterate(a, b, x, y, times[i-1])
    prediction = model(a, b, x)
    loss = loss_function(a, b, x, y)

    sh.sheet1.update(('A'+ str(i)), str(i))
    sh.sheet1.update(('B'+ str(i)), str(times[i-1]))
    sh.sheet1.update(('C'+ str(i)), str(a[0]).replace('.', ','))
    sh.sheet1.update(('D'+ str(i)), str(b[0]).replace('.', ','))
    sh.sheet1.update(('E'+ str(i)), str(loss).replace('.', ','))
    print(loss)
