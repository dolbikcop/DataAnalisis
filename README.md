# АНАЛИЗ ДАННЫХ И ИСКУССТВЕННЫЙ ИНТЕЛЛЕКТ [in GameDev]
Отчет по лабораторной работе #2 выполнил(а):
- Биккужина Полина Дмитриевна
- РИ210941
Отметка о выполнении заданий (заполняется студентом):

| Задание | Выполнение | Баллы |
| ------ |------------| ------ |
| Задание 1 | *          | 60 |
| Задание 2 | *          | 20 |
| Задание 3 | *          | 20 |

знак "*" - задание выполнено; знак "#" - задание не выполнено;

Работу проверили:
- к.т.н., доцент Денисов Д.В.
- к.э.н., доцент Панов М.А.
- ст. преп., Фадеев В.О.

Структура отчета

- Данные о работе: название работы, фио, группа, выполненные задания.
- Цель работы.
- Задание 1.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 2.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 3.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Выводы.
- ✨Magic ✨

## Цель работы
Познакомиться с программными средствами для организции
передачи данных между инструментами google, Python и Unity


## Задание 1

### Реализовать совместную работу и передачу данных в связке Python - Google-Sheets – Unity. При выполнении задания используйте видео-материалы и исходные данные, предоставленные преподавателя курса.
#### В сервисе Google Console подключила API для работы с Google Sheets и Google Drive.
![Image alt](Images/i1.jpg)

#### Реализовала запись данных из скрипта на python в google-таблицу. 
Данные описывают изменение темпа инфляции на протяжении 11 отсчётных периодов, с учётом стоимости игрового объекта в каждый период.

```py
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
```
![Image alt](Images/i2.jpg)

#### Создала новый проект на Unity. 
Он будет получать данные из google-таблицы, в которую были записаны данные в предыдущем пункте.
![Image alt](Images/i3.jpg)

#### Написала функционал на Unity, в котором будет воспризводиться аудио-файл в зависимости от значения данных из таблицы.
```csharp
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class AudioScr : MonoBehaviour
{
    [SerializeField] private AudioClip goodSpeak;
    [SerializeField] private AudioClip normalSpeak;
    [SerializeField] private AudioClip badSpeak;
    
    private AudioSource selectAudio;

    private Dictionary<string, float> dataSet = new Dictionary<string, float>();

    private bool startStatus;
    private int i = 1;

    private string webAddress =
        "https://sheets.googleapis.com/v4/spreadsheets/1DF3UfwPP3SA6qs4cogYEDMablnJhB9sTvtwOww7urhk/values/Лист1?key=AIzaSyCi4B0btrAgfmmo6WrU_WirZjNUSQrBNKs";
    void Start()
    {
        StartCoroutine(GoogleSheets());
        selectAudio = GetComponent<AudioSource>();
    }

    private IEnumerator GoogleSheets()
    {
        UnityWebRequest currentResp = UnityWebRequest.Get(webAddress);
        yield return currentResp.SendWebRequest();

        string rawResp = currentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);

        foreach (var itemRJ in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRJ.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add("Mon_" + selectRow[0], float.Parse(selectRow[2]));
        }
    }
    
    void Update()
    {
        if (!startStatus && i != dataSet.Count+1)
        {
            switch (dataSet["Mon_" + i.ToString()])
            {
                case <=10: 
                    StartCoroutine(PlaySelectAudioGood());
                    break;
                case >=100:
                    StartCoroutine(PlaySelectAudioBad());
                    break;
                default:
                    StartCoroutine(PlaySelectAudioNormal());
                    break;
            }
            Debug.Log(dataSet["Mon_" + i.ToString()]);   
        }
    }
    
    IEnumerator PlaySelectAudioGood()
    {
        startStatus = true;
        selectAudio.clip = goodSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        i++;
        startStatus = false;
    }
    IEnumerator PlaySelectAudioNormal()
    {
        startStatus = true;
        selectAudio.clip = normalSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        i++;
        startStatus = false;
    }
    IEnumerator PlaySelectAudioBad()
    {
        startStatus = true;
        selectAudio.clip = badSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        i++;
        startStatus = false;
    }
}
```
![Image alt](Images/i4.jpg)


## Задание 2
### Реализовать запись в Google-таблицу набора данных, полученных с помощью линейной регрессии из лабораторной работы № 1
Совместила код из первой лабораторной и первого задания этой работы.
```py
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
    sh.sheet1.update(('C'+ str(i)), str(a[0]))
    sh.sheet1.update(('D'+ str(i)), str(b[0]))
    sh.sheet1.update(('E'+ str(i)), str(loss))
    print(loss)
```
Результат его работы:
![Image alt](Images/i5.jpg)
![Image alt](Images/i6.jpg)

## Задание 3
### Самостоятельно разработать сценарий воспроизведения звукового сопровождения в Unity в зависимости от изменения считанных данных в задании 2
Пропарсила данные из задания 2, а потом подредактировала сценарий воспроизведения аудио.
```csharp
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class AudioScr : MonoBehaviour
{
    [SerializeField] private AudioClip goodSpeak;
    [SerializeField] private AudioClip normalSpeak;
    [SerializeField] private AudioClip badSpeak;
    
    private AudioSource selectAudio;

    private Dictionary<string, float> dataSet = new Dictionary<string, float>();

    private string webAddress =
        "https://sheets.googleapis.com/v4/spreadsheets/1DF3UfwPP3SA6qs4cogYEDMablnJhB9sTvtwOww7urhk/values/Лист1?key=AIzaSyCi4B0btrAgfmmo6WrU_WirZjNUSQrBNKs";
    void Start()
    {
        StartCoroutine(GoogleSheets());
        selectAudio = GetComponent<AudioSource>();
        StartCoroutine(PlaySelectAudio());
    }
    

    private IEnumerator GoogleSheets()
    {
        UnityWebRequest currentResp = UnityWebRequest.Get(webAddress);
        yield return currentResp.SendWebRequest();

        string rawResp = currentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);

        foreach (var itemRJ in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRJ.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add(selectRow[0], float.Parse(selectRow[4]));
        }
    }
    
    IEnumerator PlaySelectAudio()
    {
        yield return new WaitForSeconds(3);
        for (int i = 1; i < dataSet.Count + 1; i++)
        {
            switch (dataSet[i.ToString()])
            {
                case <=200: 
                    selectAudio.clip = goodSpeak;
                    break;
                case >=2000:
                    selectAudio.clip = badSpeak;
                    break;
                default:
                    selectAudio.clip = normalSpeak;
                    break;
            }
            selectAudio.Play();
            yield return new WaitForSeconds(3);
            Debug.Log(dataSet[i.ToString()]); 
        }
    }
}
```
Констольный вывод:
![Image alt](Images/i7.jpg)
## Выводы
В ходе работы я ознакомилась с методом записи и чтения данных из Google Sheets. 
Этот способ может пригодиться для чтения и обработки больших данных в играх.
#### ✨✨✨