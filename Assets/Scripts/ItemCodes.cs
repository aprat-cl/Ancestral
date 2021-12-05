using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCodes 
{
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
    TopCount

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
    ItemCodes Code;
    string IconName;
    string Name;
    float Value;

    public ItemData(ItemCodes code, string iconName, string name, float value)
    {
        this.Code = code;
        this.IconName = iconName;
        this.Name = name;
        this.Value = value;
    }
}



