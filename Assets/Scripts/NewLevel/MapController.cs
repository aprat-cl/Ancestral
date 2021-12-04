using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public int Width, Height;    
    public Texture2D texttest;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[,] icons = new GameObject[Width, Height];

        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                icons[x, y] = (GameObject)LoadPrefabFromFile("Icon");
                icons[x, y].transform.SetParent(transform);
                var ic = icons[x, y].GetComponent<IconController>();
                ic.PosX = x;
                ic.PosY = y;
                icons[x, y].transform.position = new Vector3(x * 10, y * 10);
                var ri = icons[x, y].transform.Find("Image").GetComponent<RawImage>();
                ri.texture = texttest;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMapIcon(RoomController room)
    {        
        
    }

    internal static void addIcon(RoomInstance room)
    {
        
    }
    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        //Debug.Log("Trying to load LevelPrefab from file (" + filename + ")...");
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }
}
