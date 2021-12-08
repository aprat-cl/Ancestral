using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SavePlayerData 
{
    public float PlayerHealth, PlayerMana, Gold, PlayerDamage, ManaUsage;
    public RoomInstance CurrentRoom;
    public float MapLocX, MapLocY, AttSpeed;
    public string parentScene;
    public bool SceneLoaded;
    public SavedMap CurrRoomSpec;
    public float Level, Attackpow, Defense, MagicDefense;
    public Weapon weapon;
    public string PlayerModelName;
    public List<ItemBagSaveData> BagItems;

    public SavePlayerData()
    {
        PlayerHealth = PlayerData.PlayerHealth;
        PlayerMana = PlayerData.PlayerMana;
        Gold = PlayerData.Gold;
        PlayerDamage = PlayerData.PlayerDamage;
        ManaUsage = PlayerData.ManaUsage;
        CurrentRoom = PlayerData.CurrentRoom;
        MapLocX = PlayerData.MapLocX;
        MapLocY = PlayerData.MapLocY;
        AttSpeed = PlayerData.AttSpeed;
        parentScene = PlayerData.parentScene;
        SceneLoaded = PlayerData.SceneLoaded;
        CurrRoomSpec = new SavedMap(PlayerData.CurrRoomSpec);
        Level = PlayerData.Level;
        Attackpow = PlayerData.Attackpow;
        Defense = PlayerData.Defence;
        MagicDefense = PlayerData.MagicDef;
        weapon = PlayerData.weapon;
        PlayerModelName = PlayerData.PlayerModelName;
        BagItems = new List<ItemBagSaveData>();
        foreach (var o in PlayerData.Bag)
        {
            BagItems.Add(new ItemBagSaveData() { code = o.Key, count = o.Value });
        }

    }
    public void SetLoadData()
    {
        PlayerData.CurrRoomSpec = new MapGenerated();
        PlayerData.Bag = new Dictionary<ItemCodes, int>();

        PlayerData.PlayerHealth = PlayerHealth;
        PlayerData.PlayerMana = PlayerMana;
        PlayerData.Gold = Gold;
        PlayerData.PlayerDamage = PlayerDamage;
        PlayerData.ManaUsage = ManaUsage;
        PlayerData.CurrentRoom = CurrentRoom;
        PlayerData.MapLocX = MapLocX;
        PlayerData.MapLocY = MapLocY;
        PlayerData.AttSpeed = AttSpeed;
        PlayerData.parentScene = parentScene;
        PlayerData.SceneLoaded = SceneLoaded;
        PlayerData.CurrRoomSpec = CurrRoomSpec.GenerateFromSaved();
        PlayerData.Level = Level;
        PlayerData.Attackpow = Attackpow;
        PlayerData.Defence = Defense;
        PlayerData.MagicDef = MagicDefense;
        PlayerData.weapon = weapon;
        PlayerData.PlayerModelName = PlayerModelName;
        PlayerData.bIsJumping = false;
        PlayerData.SceneLoaded = true;
        PlayerData.LoadSaveData = true;
        
        foreach(var o in BagItems)
        {
            PlayerData.Bag.Add(o.code, o.count);
        }
    }

}
[Serializable]
public class SavedMap
{
    public List<List<RoomInstance>> layout;
    public List<List<bool>> exits;
    public Location StairUpLocation = new Location(Vector2.zero);
    public Location StairDownLocation = new Location(Vector2.zero);
    public SavedMap(MapGenerated map)
    {
        layout = new List<List<RoomInstance>>();
        for (int y = 0; y < map.layout.GetLength(1); y++)
        {
            var lst = new List<RoomInstance>();
            for (int x = 0; x < map.layout.GetLength(0); x++)
            {
                lst.Add(map.layout[x, y]);
            }
            layout.Add(lst);
        }
        exits = new List<List<bool>>();
        for (int y = 0; y < map.layout.GetLength(1); y++)
        {
            var lst = new List<bool>();
            for (int x = 0; x < map.layout.GetLength(0); x++)
            {
                lst.Add(map.exits[x, y]);
            }
            exits.Add(lst);
        }
        StairUpLocation = new Location(map.StairUpLocation);
        StairDownLocation = new Location(map.StairDownLocation);
    }
    public MapGenerated GenerateFromSaved()
    {
        MapGenerated map = new MapGenerated();
        map.layout = new RoomInstance[layout[0].Count, layout.Count];
        for (int y = 0; y < layout.Count ; y++)
        {
            for (int x = 0; x < layout[y].Count; x++)
            {
                map.layout[x, y] = layout[y][x];
            }
        }
        map.exits = new bool[exits[0].Count, exits.Count];
        for (int y = 0; y < exits.Count; y++)
        {
            for (int x = 0; x < exits[y].Count; x++)
            {
                map.exits[x, y] = exits[y][x];
            }
        }
        map.StairDownLocation = StairDownLocation.ToVector2();
        map.StairUpLocation = StairUpLocation.ToVector2();

        return map;
    }
    [Serializable]
    public class Location
    {
        public float x, y;
        public Location(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Location(Vector2 point)
        {
            this.x = point.x;
            this.y = point.y;
        }

        public Vector2 ToVector2()
        {
            Vector2 value = new Vector2(x, y);
            return value;
        }
    }
}