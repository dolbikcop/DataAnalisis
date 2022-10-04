using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class AudioScr : MonoBehaviour
{
    [SerializeField] private AudioClip normalSpeak;
    [SerializeField] private AudioClip badSpeak;
    [SerializeField] private AudioClip selectSpeak;

    private Dictionary<string, float> dataSet = new Dictionary<string, float>();

    private bool startStatus = false;
    private int i = 1;

    private string webAddress =
        "https://sheets.googleapis.com/v4/spreadsheets/1DF3UfwPP3SA6qs4cogYEDMablnJhB9sTvtwOww7urhk/values/Лист1?key=AIzaSyCi4B0btrAgfmmo6WrU_WirZjNUSQrBNKs";
    void Start()
    {
        StartCoroutine(GoogleSheets());
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
        Debug.Log(dataSet["Mon_1"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
