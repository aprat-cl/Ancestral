using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCodes 
{
    //ItemSection
    None,
    Stone, 
    Flint,
    Bone,
    Shell,
    Junk,
    WoodChip,
    TinChip,
    IronChip,
    CopperChip,
    SteelChip,
    SilverChip,
    GoldChip,
    LapisChip,
    SaphireChip,
    EmeraldChip,
    RubyChip,
    CrystalChip,
    DiamondChip,
    MithrilChip,
    OricalcumChip,
    HellChip,
    SkyChip,
    DarkChip,
    LightChip,
    OmniChip,
    Lumber,
    TinIngot,
    IronIngot,
    CopperIngot,
    SteelIngot,
    SilverIngot,
    GoldIngot,
    LapisIngot,
    SaphireIngot,
    EmeraldIngot,
    RubyIngot,
    CrystalIngot,
    DiamondIngot,
    MithrilIngot,
    OricalcumIngot,
    HellIngot,
    SkyIngot,
    DarkIngot,
    LightIngot, 
    OmniIngot,
    TopCount,
    //MeleeSection
    Stick,
    WoodSword,
    TinSword,
    IronSword,
    //BowsSection
    WoodBow,    
    IronBow,
    //StaffSection
    Wand,
    TinStaff,
    IronStaff,
    //ArmourSection
    Shirt,
    WoodArm,
    TinArm,
    IronArm

}
public enum ItemQuality
{
    Normal,
    Uncommon,
    Rare,
    Special,
    Legendary
}

public class ItemData
{
    public ItemCodes Code;
    public string IconName;
    public string Name;
    public float Value;
    public ItemType type = ItemType.Item;

    public ItemData(ItemCodes code, string iconName, string name, float value)
    {
        this.Code = code;
        this.IconName = iconName;
        this.Name = name;
        this.Value = value;
    }
}
public enum ItemType
{
    Valuable,
    Item,
    Weapon,
    Key
}
[Serializable]
public class ItemBagSaveData
{
    public ItemCodes code;
    public int count;

}



