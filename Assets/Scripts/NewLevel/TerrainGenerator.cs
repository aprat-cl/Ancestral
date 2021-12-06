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
        fillItemData();
        

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
            PlayerData.AttSpeed = 5f;
            PlayerData.Defense = 1;
            PlayerData.MagicDefense = 1;
            PlayerData.PlayerMana = 50;
            PlayerData.PlayerHealth = 20;

            PlayerData.weapon = new Weapon();
            PlayerData.weapon.Power1 = 4;
            PlayerData.weapon.Power2 = 6;
            PlayerData.weapon.PowerSpecial = 12;
            PlayerData.weapon.SpecialManaUsage = 10;
            PlayerData.weapon.Pow1Usage = 0;
            PlayerData.weapon.Pow2Usage = 5;

            PlayerData.CurrRoomSpec.StairDownLocation = new Vector2(-1, -1);
            PlayerData.CurrRoomSpec.StairUpLocation = new Vector2(UnityEngine.Random.Range(0, LevelWidth + 1), UnityEngine.Random.Range(0, LevelHeight + 1));

            PlayerData.Bag = new Dictionary<ItemCodes, int>();

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

    private void fillItemData()
    {
        PlayerData.ItemCollection = new Dictionary<ItemCodes, ItemData>();
        PlayerData.ItemCollection.Add(ItemCodes.None, new ItemData(ItemCodes.None, "generic", "None", 0));
        PlayerData.ItemCollection.Add(ItemCodes.Stone, new ItemData(ItemCodes.Stone, "generic", "Stone", 0));
        PlayerData.ItemCollection.Add(ItemCodes.Flint, new ItemData(ItemCodes.Flint, "generic", "Flint", 0));
        PlayerData.ItemCollection.Add(ItemCodes.Bone, new ItemData(ItemCodes.Bone, "generic", "Bone", 0));
        PlayerData.ItemCollection.Add(ItemCodes.Shell, new ItemData(ItemCodes.Shell, "generic", "Shell", 0));
        PlayerData.ItemCollection.Add(ItemCodes.Junk, new ItemData(ItemCodes.Junk, "generic", "Junk", 0));
        PlayerData.ItemCollection.Add(ItemCodes.WoodChip, new ItemData(ItemCodes.WoodChip, "generic", "Wood Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.TinChip, new ItemData(ItemCodes.TinChip, "generic", "Tin Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.IronChip, new ItemData(ItemCodes.IronChip, "generic", "Iron Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.CopperChip, new ItemData(ItemCodes.CopperChip, "generic", "Copper Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SteelChip, new ItemData(ItemCodes.SteelChip, "generic", "Steel Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SilverChip, new ItemData(ItemCodes.SilverChip, "generic", "Silver Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.GoldChip, new ItemData(ItemCodes.GoldChip, "generic", "Gold Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.LapisChip, new ItemData(ItemCodes.LapisChip, "generic", "Lapis Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SaphireChip, new ItemData(ItemCodes.SaphireChip, "generic", "Saphire Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.EmeraldChip, new ItemData(ItemCodes.EmeraldChip, "generic", "Emerald Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.RubyChip, new ItemData(ItemCodes.RubyChip, "generic", "Ruby Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.CrystalChip, new ItemData(ItemCodes.CrystalChip, "generic", "Crystal Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.DiamondChip, new ItemData(ItemCodes.DiamondChip, "generic", "Diamond Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.MithrilChip, new ItemData(ItemCodes.MithrilChip, "generic", "Mithril Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.OricalcumChip, new ItemData(ItemCodes.OricalcumChip, "generic", "Oricalcum Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.HellChip, new ItemData(ItemCodes.HellChip, "generic", "HellStone Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SkyChip, new ItemData(ItemCodes.SkyChip, "generic", "SkyPiece Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.DarkChip, new ItemData(ItemCodes.DarkChip, "generic", "Dark Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.LightChip, new ItemData(ItemCodes.LightChip, "generic", "Light Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.OmniChip, new ItemData(ItemCodes.OmniChip, "generic", "Omni Chip", 0));
        PlayerData.ItemCollection.Add(ItemCodes.Lumber, new ItemData(ItemCodes.Lumber, "generic", "Lumber", 0));
        PlayerData.ItemCollection.Add(ItemCodes.TinIngot, new ItemData(ItemCodes.TinIngot, "generic", "Tin Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.IronIngot, new ItemData(ItemCodes.IronIngot, "generic", "Iron Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.CopperIngot, new ItemData(ItemCodes.CopperIngot, "generic", "Copper Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SteelIngot, new ItemData(ItemCodes.SteelIngot, "generic", "Steel Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SilverIngot, new ItemData(ItemCodes.SilverIngot, "generic", "Silver Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.GoldIngot, new ItemData(ItemCodes.GoldIngot, "generic", "Gold Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.LapisIngot, new ItemData(ItemCodes.LapisIngot, "generic", "Lapis Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SaphireIngot, new ItemData(ItemCodes.SaphireIngot, "generic", "Saphire Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.EmeraldIngot, new ItemData(ItemCodes.EmeraldIngot, "generic", "Emerald Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.RubyIngot, new ItemData(ItemCodes.RubyIngot, "generic", "Ruby Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.CrystalIngot, new ItemData(ItemCodes.CrystalIngot, "generic", "Crystal Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.DiamondIngot, new ItemData(ItemCodes.DiamondIngot, "generic", "Diamond Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.MithrilIngot, new ItemData(ItemCodes.MithrilIngot, "generic", "Mithril Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.OricalcumIngot, new ItemData(ItemCodes.OricalcumIngot, "generic", "Oricalcum Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.HellIngot, new ItemData(ItemCodes.HellIngot, "generic", "HellStone Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.SkyIngot, new ItemData(ItemCodes.SkyIngot, "generic", "SkyPiece Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.DarkIngot, new ItemData(ItemCodes.DarkIngot, "generic", "DarkMatter Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.LightIngot, new ItemData(ItemCodes.LightIngot, "generic", "Light Ingot", 0));
        PlayerData.ItemCollection.Add(ItemCodes.OmniIngot, new ItemData(ItemCodes.OmniIngot, "generic", "Omni Ingot", 0));

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
