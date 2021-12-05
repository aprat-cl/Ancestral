using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public static float PlayerHealth, PlayerMana, Gold, PlayerDamage, ManaUsage;
    public static RoomInstance CurrentRoom;
    public static float MapLocX, MapLocY, AttSpeed;
    public static string parentScene;
    public static bool SceneLoaded, bIsJumping;
    public static MapGenerated CurrRoomSpec;
    public static float Level, Attackpow, Defense, MagicDefense;
    public static Weapon weapon;
    public static string PlayerModelName;
    public static bool LoadSaveData = false;
    public static Hashtable Bag;
    public static Hashtable ItemCollection;
    

}
[Serializable]
public class Weapon
{
    public string Name;
    public float Power1, Power2, PowerSpecial;
    public float SpecialManaUsage, Pow1Usage, Pow2Usage;
    public WeaponType type;
}
public enum WeaponType
{
    Melee, Ranged, MagicMele, MagicRanged
}
[Serializable]
public class MapGenerated
{
    public RoomInstance[,] layout;
    public bool[,] exits;
    public Vector2 StairUpLocation = Vector2.zero;
    public Vector2 StairDownLocation = Vector2.zero;

}
