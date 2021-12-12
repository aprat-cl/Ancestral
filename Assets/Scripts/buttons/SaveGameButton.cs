using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using Newtonsoft.Json;

public class SaveGameButton : MonoBehaviour
{
    public GameObject SavePanel;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SavePlayerData saveData = new SavePlayerData();

        string content = JsonConvert.SerializeObject(saveData);
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\")) 
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\");
        }
        string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\save.dat";
        string BkpPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\save.bak";
        try
        {
            if(File.Exists(SavePath)) File.Copy(SavePath, BkpPath, true);
            File.WriteAllText(SavePath, content);
            SavePanel.transform.Find("Text").GetComponent<Text>().text = "Save successful!";
            SavePanel.GetComponent<Image>().color = new Color(0.1960784f, 0.3643001f, 0.5647059f, 1);
            SavePanel.SetActive(true);
            StartCoroutine(ClosePanelAfter(2f));
        }
        catch(Exception ex)
        {
            SavePanel.transform.Find("Text").GetComponent<Text>().text = "Error saving File!";
            SavePanel.GetComponent<Image>().color = new Color(0.5647059f, 0.2530474f, 0.1960784f, 1);
            SavePanel.SetActive(true);
            StartCoroutine(ClosePanelAfter(2f));
            Debug.LogError("Cannot write Savefile data! " + ex.Message);
        }
    }

    private IEnumerator ClosePanelAfter(float time)
    {
        yield return new WaitForSeconds(time);

        SavePanel.SetActive(false);
    }

    public string Encrypt2(string prm_text_to_encrypt, string prm_key, string prm_iv)
    {
        var sToEncrypt = prm_text_to_encrypt;

        var rj = new RijndaelManaged()
        {
            Padding = PaddingMode.PKCS7,
            Mode = CipherMode.CBC,
            KeySize = 256,
            BlockSize = 256,
            //FeedbackSize = 256
        };

        var key = Convert.FromBase64String(prm_key);
        var IV = Convert.FromBase64String(prm_iv);

        var encryptor = rj.CreateEncryptor(key, IV);

        var msEncrypt = new MemoryStream();
        var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        var toEncrypt = Encoding.UTF8.GetBytes(sToEncrypt);

        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
        csEncrypt.FlushFinalBlock();

        var encrypted = msEncrypt.ToArray();

        return (Convert.ToBase64String(encrypted));
    }
}
