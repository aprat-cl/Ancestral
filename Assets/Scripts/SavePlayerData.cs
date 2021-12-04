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
    public MapGenerated CurrRoomSpec;
    public float Level, Attackpow, Defense, MagicDefense;
    public Weapon weapon;
    public string PlayerModelName;

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
        CurrRoomSpec = PlayerData.CurrRoomSpec;
        Level = PlayerData.Level;
        Attackpow = PlayerData.Attackpow;
        Defense = PlayerData.Defense;
        MagicDefense = PlayerData.MagicDefense;
        weapon = PlayerData.weapon;
        PlayerModelName = PlayerData.PlayerModelName;
    }
    public void SetLoadData()
    {
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
        PlayerData.CurrRoomSpec = CurrRoomSpec;
        PlayerData.Level = Level;
        PlayerData.Attackpow = Attackpow;
        PlayerData.Defense = Defense;
        PlayerData.MagicDefense = MagicDefense;
        PlayerData.weapon = weapon;
        PlayerData.PlayerModelName = PlayerModelName;
        PlayerData.bIsJumping = false;
        PlayerData.SceneLoaded = true;
        PlayerData.LoadSaveData = true;
    }
}
