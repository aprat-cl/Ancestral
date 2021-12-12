using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public static float PlayerHealth, PlayerMana, Gold, PlayerDamage, ManaUsage;
    public static RoomInstance CurrentRoom;
    public static float MapLocX, MapLocY, AttSpeed, AttAcc;
    public static string parentScene;
    public static bool SceneLoaded, bIsJumping;
    public static MapGenerated CurrRoomSpec;
    public static float Level, Attackpow, MagPow, Defence, MagicDef;
    public static Weapon weapon;
    public static Armour armour;
    public static string PlayerModelName;
    public static bool LoadSaveData = false;
    public static Dictionary<ItemCodes,int> Bag;
    public static Dictionary<ItemCodes, ItemData> ItemCollection;
    

}
[Serializable]
public class Weapon
{
    public ItemCodes Code;
    public string Name;
    public float Power1, Power2, PowerSpecial;
    public float SpecialManaUsage, Pow1Usage, Pow2Usage;
    public float AccBonus;
    public WeaponType type;
}
[Serializable]
public class Armour
{
    public ItemCodes Code, Material;
    public string Name;
    public float Def, MagDef;
    public float MBonus, PowBonus, ExpBonus, SpeedBonus;
    public ArmourType type;
}
public enum WeaponType
{
    Melee, Ranged, MagicMele, MagicRanged
}
public enum ArmourType
{
    Normal, Magic
}
[Serializable]
public class MapGenerated
{
    public RoomInstance[,] layout;
    public bool[,] exits;
    public TreasureRoom[,] spoils;
    public Vector2 StairUpLocation = Vector2.zero;
    public Vector2 StairDownLocation = Vector2.zero;

}
