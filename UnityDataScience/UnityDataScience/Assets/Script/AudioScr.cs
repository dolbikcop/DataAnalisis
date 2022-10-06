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
