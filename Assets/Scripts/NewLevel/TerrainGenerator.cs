using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static RoomInstance;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public int LevelWidth, LevelHeight;
    //private RoomInstance[,] rooms;
    public Canvas LoadingScr, Pannel;
    public Camera mapCamera;
    public GameObject Level, player;
    public GameObject Light;
    public RoomType tileType;
    Vector3 StartPos;
    

    void Start()
    {
        if (PlayerData.CurrRoomSpec == null)
        {
            LoadingScr.GetComponent<Canvas>().enabled = true;
            PlayerData.CurrRoomSpec = new MapGenerated();            
        }
        if (!PlayerData.SceneLoaded)
        {
            //PlayerData.CurrentRoom = RoomInstance.CreateRoom(0x0, Connector.None);
            //PlayerData.CurrentRoom.type = RoomType.Initial;
            PlayerData.PlayerModelName = "Player_default";      
            PlayerData.Level = 1;
            PlayerData.Attackpow = 1;
            PlayerData.AttSpeed = 9f;
            PlayerData.Defense = 4;
            PlayerData.Defense = 3;
            PlayerData.PlayerMana = 100;
            PlayerData.PlayerHealth = 100;

            PlayerData.weapon = new Weapon();
            PlayerData.weapon.Power1 = 10;
            PlayerData.weapon.Power2 = 12;
            PlayerData.weapon.PowerSpecial = 20;
            PlayerData.weapon.SpecialManaUsage = 20;
            PlayerData.weapon.Pow1Usage = 0;
            PlayerData.weapon.Pow2Usage = 5;

            PlayerData.CurrRoomSpec.StairDownLocation = new Vector2(-1, -1);
            PlayerData.CurrRoomSpec.StairUpLocation = new Vector2(UnityEngine.Random.Range(0, LevelWidth + 1), UnityEngine.Random.Range(0, LevelHeight + 1));

            Debug.Log("Creating new Floor!");
            StartCoroutine(ExecuteCreation(1f));
        }
        else
        {
            PlayerData.LoadSaveData = false;
            Debug.Log("Loading Floor!");
            StartCoroutine(ExecuteLoadLayout(1f));
        }        
    }

    private IEnumerator ExecuteLoadLayout(float time)
    {
        LoadingScr.GetComponent<Canvas>().enabled = true;
        for (int y = 0; y < LevelHeight; y++)
        {
            for (int x = 0; x < LevelWidth; x++)
            {
                CreateRoomObject(PlayerData.CurrRoomSpec.layout[x, y], x, y, PlayerData.CurrRoomSpec.exits[x, y], tileType);

            }
        }
        player.transform.position = new Vector3(PlayerData.MapLocX, 0.47f, PlayerData.MapLocY);
        LoadingScr.GetComponent<Canvas>().enabled = false;
        Pannel.GetComponent<Canvas>().enabled = true;

        yield return new WaitForSeconds(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ExecuteCreation(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerData.CurrRoomSpec.layout = new RoomInstance[LevelWidth, LevelHeight];

        PlayerData.CurrRoomSpec.exits = new bool[LevelWidth, LevelHeight];
        Light.GetComponent<Light>().color = GetColorFromRoomType(tileType); 

        for (int i = 0; i < LevelHeight; ++i)
            for (int j = 0; j < LevelWidth; ++j)
                PlayerData.CurrRoomSpec.exits[j, i] = UnityEngine.Random.Range(0, 10) > 8;

        for (int y = 0; y < LevelHeight; y++)
        {
            for (int x = 0; x < LevelWidth; x++)
            {
                PlayerData.CurrRoomSpec.layout[x, y] = RoomInstance.CreateRoom(Convert.ToByte(UnityEngine.Random.Range(0, 8)), Connector.None);
                if (y == LevelHeight - 1)
                {
                    PlayerData.CurrRoomSpec.layout[x, y].Up = Doors.Blocked;
                }
                if (y > 0)
                {
                    if (PlayerData.CurrRoomSpec.layout[x, y - 1].Up == Doors.Open)
                        PlayerData.CurrRoomSpec.layout[x, y].Down = Doors.Open;
                    else if (PlayerData.CurrRoomSpec.layout[x, y - 1].Up == Doors.Wide)
                        PlayerData.CurrRoomSpec.layout[x, y].Down = Doors.Wide;
                }
                else
                {
                    PlayerData.CurrRoomSpec.layout[x, y].Down = Doors.Blocked;
                }
                if (x == LevelWidth - 1)
                {
                    PlayerData.CurrRoomSpec.layout[x, y].Right = Doors.Blocked;
                }
                if (x > 0)
                {
                    if (PlayerData.CurrRoomSpec.layout[x - 1, y].Right == Doors.Open)
                        PlayerData.CurrRoomSpec.layout[x, y].Left = Doors.Open;
                    else if (PlayerData.CurrRoomSpec.layout[x - 1, y].Right == Doors.Wide)
                        PlayerData.CurrRoomSpec.layout[x, y].Left = Doors.Wide;
                }
                else
                {
                    PlayerData.CurrRoomSpec.layout[x, y].Left = Doors.Blocked;
                }
                if (x == LevelWidth / 2 && y == 0)
                {
                    PlayerData.CurrRoomSpec.layout[x, y].bStartRoom = true;
                    //rooms[x, y].Down = Doors.Open;
                }
                if (PlayerData.CurrRoomSpec.layout[x, y].Left == Doors.Close &&
                    PlayerData.CurrRoomSpec.layout[x, y].Up == Doors.Close &&
                    PlayerData.CurrRoomSpec.layout[x, y].Right == Doors.Close &&
                    PlayerData.CurrRoomSpec.layout[x, y].bStartRoom
                    )
                {
                    int seed = UnityEngine.Random.Range(1, 4);
                    if (seed == 1) PlayerData.CurrRoomSpec.layout[x, y].Left = Doors.Open;
                    if (seed == 2) PlayerData.CurrRoomSpec.layout[x, y].Up = Doors.Open;
                    if (seed == 3) PlayerData.CurrRoomSpec.layout[x, y].Right = Doors.Open;
                }
            }
        }
        for (int y = 0; y < LevelHeight; y++)
        {
            for (int x = 0; x < LevelWidth; x++)
            {
                CreateRoomObject(PlayerData.CurrRoomSpec.layout[x, y], x, y, PlayerData.CurrRoomSpec.exits[x,y], tileType); 
                
            }
        }
        player.transform.position = StartPos;
        LoadingScr.GetComponent<Canvas>().enabled = false;
        Pannel.GetComponent<Canvas>().enabled = true;

        //LoadingScr.enabled = false;
        PlayerData.SceneLoaded = true;     
    }

    private Color GetColorFromRoomType(RoomType type)
    {
        Color color;
        switch (type)
        {
            case RoomType.Initial:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.None:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Cave:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Praire:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Mountains:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Sky:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Beach:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Lava:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Hellish:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Underwater:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Void:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            case RoomType.Light:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;
            default:
                color = new Color(0.509078f, 0.6122192f, 0.754717f);
                break;

        }
        return color;
    }

    void CreateRoomObject(RoomInstance room, int posX, int posY, bool HasExit, RoomType type)
    {
        string Folder = "";
        switch (type)
        {
            case RoomType.Initial:
                Folder = "initial\\";
                break;
            case RoomType.None:
                Folder = "";
                break;
            case RoomType.Cave:
                Folder = "cave\\";
                break;
            case RoomType.Praire:
                Folder = "praire\\";
                break;
            case RoomType.Mountains:
                Folder = "mountains\\";
                break;
            case RoomType.Sky:
                Folder = "sky\\";
                break;
            case RoomType.Beach:
                Folder = "beach\\";
                break;
            case RoomType.Lava:
                Folder = "lava\\";
                break;
            case RoomType.Hellish:
                Folder = "hellish\\";
                break;
            case RoomType.Underwater:
                Folder = "water\\";
                break;
            case RoomType.Void:
                Folder = "void\\";
                break;
            case RoomType.Light:
                Folder = "light\\";
                break;
            default:
                Folder = "";
                break;

        }
        var roomResource = LoadPrefabFromFile(Folder + "Room");
        var instance = (GameObject)Instantiate(roomResource, Vector3.zero, Quaternion.identity);
        var rc = instance.GetComponent<RoomController>();
        instance.transform.parent = Level.transform;

        if(room.Left == Doors.Open)
        {
            Destroy(instance.transform.Find("Door_L").gameObject);
        }
        else if (room.Left == Doors.Wide)
        {
            Destroy(instance.transform.Find("Door_L").gameObject);
            Destroy(instance.transform.Find("Wall_LU").gameObject);
            Destroy(instance.transform.Find("Wall_LD").gameObject);
        }
        else if(room.Left == Doors.Blocked)
        {
            instance.transform.Find("Door_L").gameObject.tag = "wall";
        }
        if (room.Up == Doors.Open)
        {
            Destroy(instance.transform.Find("Door_U").gameObject);
        }
        else if (room.Up == Doors.Wide)
        {
            Destroy(instance.transform.Find("Door_U").gameObject);
            Destroy(instance.transform.Find("Wall_UL").gameObject);
            Destroy(instance.transform.Find("Wall_UR").gameObject);
        }
        else if(room.Up == Doors.Blocked)
        {
            instance.transform.Find("Door_U").gameObject.tag = "wall";
        }
        if (room.Right == Doors.Open)
        {
            Destroy(instance.transform.Find("Door_R").gameObject);
        }
        else if (room.Right == Doors.Wide)
        {
            Destroy(instance.transform.Find("Door_R").gameObject);
            Destroy(instance.transform.Find("Wall_RU").gameObject);
            Destroy(instance.transform.Find("Wall_RD").gameObject);
        }
        else if(room.Right == Doors.Blocked)
        {
            instance.transform.Find("Door_R").gameObject.tag = "wall";
        }
        if (room.Down == Doors.Open)
        {
            Destroy(instance.transform.Find("Door_D").gameObject);
        }
        else if (room.Down == Doors.Wide)
        {
            Destroy(instance.transform.Find("Door_D").gameObject);
            Destroy(instance.transform.Find("Wall_DR").gameObject);
            Destroy(instance.transform.Find("Wall_DL").gameObject);
        }
        else if(room.Down == Doors.Blocked)
        {
            instance.transform.Find("Door_D").gameObject.tag = "wall";
        }
        if (!HasExit || room.bStartRoom)
        {
            Destroy(instance.transform.Find("Teleport").gameObject);
        }
        if (!room.bStartRoom)
        {
            Destroy(instance.transform.Find("Arrival").gameObject);
        }
        if(Mathf.RoundToInt(PlayerData.CurrRoomSpec.StairUpLocation.x) == posX && Mathf.RoundToInt(PlayerData.CurrRoomSpec.StairUpLocation.y) == posY)
        {
            instance.transform.Find("StairUp").gameObject.SetActive(true);
        }
        else
        {
            instance.transform.Find("StairUp").gameObject.SetActive(false);
        }
        

        Texture2D mapicon = GetMapIconRoom(room);
        room.Icon = mapicon;

        rc.room = room;

        instance.transform.position = new Vector3(posX * 20, -0.5f, posY * 20);
        //instance.tag = posX + "." + posY;
        if (room.bStartRoom)
        {
            StartPos = new Vector3((posX * 20), 0.47f, (posY * 20) - 5);
        }
    }

    private Texture2D GetMapIconRoom(RoomInstance room)
    {
        if (room.Left == Doors.Open && room.Up == Doors.Open && room.Right == Doors.Open && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\lurd.png");
        if (room.Left == Doors.Open && room.Up == Doors.Open && room.Right == Doors.Open && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\lur.png");
        if (room.Left == Doors.Open && room.Up == Doors.Open && room.Right == Doors.Close && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\lud.png");
        if (room.Left == Doors.Open && room.Up == Doors.Open && room.Right == Doors.Close && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\lu.png");
        if (room.Left == Doors.Open && room.Up == Doors.Close && room.Right == Doors.Open && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\lrd.png");
        if (room.Left == Doors.Open && room.Up == Doors.Close && room.Right == Doors.Open && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\lr.png");
        if (room.Left == Doors.Open && room.Up == Doors.Close && room.Right == Doors.Close && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\ld.png");
        if (room.Left == Doors.Open && room.Up == Doors.Close && room.Right == Doors.Close && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\l.png");
        if (room.Left == Doors.Close && room.Up == Doors.Open && room.Right == Doors.Open && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\urd.png");
        if (room.Left == Doors.Close && room.Up == Doors.Open && room.Right == Doors.Open && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\ur.png");
        if (room.Left == Doors.Close && room.Up == Doors.Open && room.Right == Doors.Close && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\ud.png");
        if (room.Left == Doors.Close && room.Up == Doors.Open && room.Right == Doors.Close && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\u.png");
        if (room.Left == Doors.Close && room.Up == Doors.Close && room.Right == Doors.Open && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\rd.png");
        if (room.Left == Doors.Close && room.Up == Doors.Close && room.Right == Doors.Open && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\r.png");
        if (room.Left == Doors.Close && room.Up == Doors.Close && room.Right == Doors.Close && room.Down == Doors.Open) return LoadPNG(@".\Assets\Sprites\d.png");
        if (room.Left == Doors.Close && room.Up == Doors.Close && room.Right == Doors.Close && room.Down == Doors.Close) return LoadPNG(@".\Assets\Sprites\0.png");
        return LoadPNG(@".\Assets\Sprites\0.png");


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
    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }       
}
