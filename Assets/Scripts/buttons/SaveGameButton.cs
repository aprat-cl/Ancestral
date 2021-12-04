using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

public class SaveGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SavePlayerData saveData = new SavePlayerData();
        
        string content = JsonUtility.ToJson(saveData);
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\")) 
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\");
        }
        string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\save.dat";
        string BkpPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AncestralGame\\SaveData\\save.bak";
        try
        {
            if(File.Exists(SavePath)) File.Copy(SavePath, BkpPath);
            File.WriteAllText(SavePath, content);
        }
        catch(Exception ex)
        {
            Debug.LogError("Cannot write Savefile data! " + ex.Message);
        }
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
