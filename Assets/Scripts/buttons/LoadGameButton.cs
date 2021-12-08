using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LoadGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        string content = "";
        string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\save.dat";
        try
        {
            if (File.Exists(SavePath)) content = File.ReadAllText(SavePath);
            SavePlayerData data = (SavePlayerData)JsonConvert.DeserializeObject(content, typeof(SavePlayerData));

            data.SetLoadData();
            Debug.Log("SaveData Loaded Successful!");
        }
        catch (Exception ex)
        {
            Debug.LogError("Cannot read Savefile data! " + ex.Message);
        }
    }
}
